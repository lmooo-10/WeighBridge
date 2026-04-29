using WeighBridge.ViewModels.Base;

namespace WeighBridge.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public int    TodaysOps     { get; } = 47;
        public string TodaysDelta   { get; } = "+12% vs yesterday";
        public int    ImportOps     { get; } = 29;
        public string ImportDelta   { get; } = "+5% vs yesterday";
        public int    GlobalPorts   { get; } = 90;
        public int    Countries     { get; } = 41;
        public string TotalTEU      { get; } = "2.6M";
        public string Uptime        { get; } = "99.8%";

        // Live ticker items
        public List<string> TickerItems { get; } = new()
        {
            "Jebel Ali  1,840 containers",
            "WB-01 Weight  28,450 kg — STABLE",
            "MSC AURORA  ETA Algiers 14:30",
            "Rotterdam  892 TEU · 98.4% on-time",
            "Port Klang  621 TEU outbound",
            "Santos, BR  Import surge +18%"
        };
    }
}
