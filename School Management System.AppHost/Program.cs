var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.School_Management_System>("SMS");

builder.AddProject<Projects.SchoolManagementSystem_Domain>("schoolmanagementsystem-domain");

builder.Build().Run();
