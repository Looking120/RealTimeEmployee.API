using System.Reflection;

namespace RealTimeEmployee.BusinessLogic;

public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
