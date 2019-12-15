using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("NDepend", "ND1000:AvoidTypesTooBig", Target = "MyBlog.Engine.Services.DataService", Justification = "It include data methodes for all services.")]
[assembly: SuppressMessage("NDepend", "ND1001:AvoidTypesWithTooManyMethods", Target = "MyBlog.Engine.Services.DataService", Justification = "TODO")]
[assembly: SuppressMessage("NDepend", "ND1001:AvoidTypesWithTooManyMethods", Target = "MyBlog.Engine.Services.MetaWeblogService", Justification = "TODO")]