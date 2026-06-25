using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NobatDehi.API.Data;
using NobatDehi.API.Models;
using NobatDehi.API.Services;

namespace NobatDehi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentController(AppDbContext context, CodeValidationService codeValidationService) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly CodeValidationService _codeValidationService = codeValidationService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var appointments = await _context.Appointments
        .Include(p => p.Plan)
        .Include(o => o.Office!)
            .ThenInclude(o => o.Province)
        .ToListAsync();
        return Ok(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Appointment appointment)
    {
        var plan = await _context.Plans.FindAsync(appointment.PlanId);
        if (plan != null)
        {
            // Validation of plan date range
            if (appointment.AppointmentDate < plan.StartDate || appointment.AppointmentDate > plan.EndDate)
                return BadRequest("تاریخ نوبت خارج از بازه زمانی طرح است");

            // Validation of the date of appointment
            if (appointment.AppointmentDate <= DateOnly.FromDateTime(DateTime.Today)) 
                return BadRequest("تاریخ نوبت مجاز نمی باشد.");  
            
            if (_codeValidationService.CheckCode(appointment.ForeignerCode, plan.CodeType)) // Validation of foreign code
            {
                var existingCount = await _context.Appointments
                    .CountAsync(a => a.ForeignerCode == appointment.ForeignerCode
                                && a.PlanId == appointment.PlanId);

                if (existingCount > 0 && !plan.AllowDuplicate) // Validation of second appointment
                    return BadRequest("این کد قبلاً در این طرح نوبت گرفته است");

                if (existingCount >= plan.MaxDuplicateCount && plan.AllowDuplicate) // Validation of maximum number of appointments
                    return BadRequest("این کد به حداکثر تعداد نوبت رسیده است");


                // If Appointment was already token
                var exists = await _context.Appointments
                            .AnyAsync(o => o.OfficeId == appointment.OfficeId
                                    && o.AppointmentDate == appointment.AppointmentDate
                                    && o.AppointmentTime == appointment.AppointmentTime);
                if(exists)
                    return BadRequest("این نوبت قبلا ثبت شده است.");

                var holiday = await _context.Holidays.
                    FirstOrDefaultAsync(h => h.Date == appointment.AppointmentDate);
                var hasException = await _context.OfficeHolidayExceptions
                    .Include(e => e.Holiday) // Check if it is a exception by office
                    .AnyAsync(e => e.OfficeId == appointment.OfficeId && e.Holiday.Date == appointment.AppointmentDate); 

                if (holiday != null && !hasException)
                    return BadRequest("این روز تعطیل است و این دفتر در این روز کار نمیکند");

                var officeSetting = await _context.OfficeSettings
                    .FirstOrDefaultAsync(o => o.OfficeId == appointment.OfficeId);
                if (officeSetting == null)
                    return BadRequest("تنظیمات دفتر پیدا نشد");

                int minute = officeSetting.AppointmentDuration;
                exists = await _context.Appointments.AnyAsync(o =>
                            o.OfficeId == appointment.OfficeId &&
                            o.AppointmentDate == appointment.AppointmentDate &&
                            o.AppointmentTime >= appointment.AppointmentTime.AddMinutes(-minute) &&
                            o.AppointmentTime <= appointment.AppointmentTime.AddMinutes(minute)
                        );
                if (exists)
                    return BadRequest("در بازه زمانی نوبت دیگری ثبت شده است.");

                // گرفتن روز های هفته نوبت (0=شنبه, 1=بکشنبه, ... 6=جمعه)
                var dayOfWeek = (int)appointment.AppointmentDate.DayOfWeek;

                // چک کردن که این روز توی روزهای کاری دفتره
                var workingDays = officeSetting.WorkingDays.Split(',').Select(int.Parse).ToList();

                if (!workingDays.Contains(dayOfWeek))
                    return BadRequest("دفتر در این روز کار نمیکند");    



                
                
                // Validation of capacity of office
                var appointmentCount = await _context.Appointments
                    .CountAsync(o => o.OfficeId == appointment.OfficeId
                    && o.AppointmentDate == appointment.AppointmentDate);
                if (appointmentCount >= officeSetting.DailyCapacity)
                    return BadRequest("ظرفیت این اداره در این روز تکمیل است.");

                // Validation of plan dependencies
                var dependencies = await _context.PlanDependencies.
                    Where(o => o.PlanId == appointment.PlanId).ToListAsync(); 

                foreach (var dependency in dependencies)
                {
                    var hasCompleted = await _context.Appointments
                        .AnyAsync(o => o.ForeignerCode == appointment.ForeignerCode
                                    && o.PlanId == dependency.RequiredPlanId);

                    if (!hasCompleted)
                    {
                        return BadRequest("ابتدا بایستی در طرح های قبلی شرکت کرده باشید.");
                    }                
                }

                // Everything is ok, create the appointment
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("کد وارد شده معتبر نیست.");
            }
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedAppointment = await _context.Appointments.FindAsync(id);
        if (deletedAppointment != null)
        {
            var appointmentDateTime = deletedAppointment.AppointmentDate
                .ToDateTime(deletedAppointment.AppointmentTime);

            var timeLeft = appointmentDateTime - DateTime.Now;

            if (timeLeft.TotalHours < 48)
                return BadRequest("لغو نوبت فقط تا 48 ساعت قبل امکان‌پذیر است");

            _context.Appointments.Remove(deletedAppointment);
            await _context.SaveChangesAsync();
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}


