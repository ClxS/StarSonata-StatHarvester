namespace StatHarvester.Utility.SerilogEnrichers
{
    using System.Diagnostics;
    using Serilog;
    using Serilog.Core;
    using Serilog.Events;

    public class CallerEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var skip = 3;
            while (true)
            {
                var stack = new StackFrame(skip);
                if (!stack.HasMethod())
                {
                    logEvent.AddPropertyIfAbsent(new LogEventProperty("Caller", new ScalarValue("<unknown method>")));
                    return;
                }

                var method = stack.GetMethod();
                if (method.DeclaringType.Assembly != typeof(Log).Assembly)
                {
                    var ln = stack.GetFileLineNumber();
                    var caller =
                        $"{method.DeclaringType.Name}.{method.Name}";
                    logEvent.AddPropertyIfAbsent(new LogEventProperty("Caller", new ScalarValue(caller)));
                    logEvent.AddPropertyIfAbsent(new LogEventProperty("Line", new ScalarValue(ln)));
                }

                skip++;
            }
        }
    }
}