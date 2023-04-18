namespace QuizManagement.Api.Authorization;

[AttributeUsage(AttributeTargets.Method)]
public class OnlyAdminAttribute : Attribute
{ }