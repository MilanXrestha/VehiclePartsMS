using Microsoft.EntityFrameworkCore;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Application.Interfaces.IServices;
using VehiclePartsMS.Infrastructure.Persistance;
using VehiclePartsMS.Infrastructure.Repositories;
using VehiclePartsMS.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("defaultConnection")));

// ── Repositories ──────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository,        UserRepository>();
builder.Services.AddScoped<IVendorRepository,      VendorRepository>();
builder.Services.AddScoped<IVehiclePartRepository, VehiclePartRepository>();
builder.Services.AddScoped<ICustomerRepository,    CustomerRepository>();
builder.Services.AddScoped<ISalesInvoiceRepository, SalesInvoiceRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IPartRequestRepository, PartRequestRepository>();
builder.Services.AddScoped<IReviewRepository,      ReviewRepository>();

// ── Services ──────────────────────────────────────────────────
builder.Services.AddScoped<IUserService,         UserService>();
builder.Services.AddScoped<IVendorService,       VendorService>();
builder.Services.AddScoped<IVehiclePartService,  VehiclePartService>();
builder.Services.AddScoped<ICustomerService,     CustomerService>();
builder.Services.AddScoped<ISalesInvoiceService, SalesInvoiceService>();
builder.Services.AddScoped<IAppointmentService,  AppointmentService>();
builder.Services.AddScoped<IPartRequestService,  PartRequestService>();
builder.Services.AddScoped<IReviewService,       ReviewService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
