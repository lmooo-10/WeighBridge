using System;
using System.Collections.Generic;
using System.Text;
using WeighBridge.Services;

namespace WeighBridge
{
    // ═══════════════════════════════════════════════════════════
    //  ServiceLocator  (Simple DI without external framework)
    //
    //  HOW TO SWITCH FROM MOCK → REAL BACKEND:
    //
    //  Step 1 — Create your real service:
    //    public class ApiWeighmentService : IWeighmentService { ... }
    //
    //  Step 2 — Change ONE line here:
    //    WeighmentService = new ApiWeighmentService("https://your-api.com");
    //
    //  Step 3 — Done. All ViewModels automatically use the real service.
    //
    //  For SQL Server / MySQL:
    //    public class DbWeighmentService : IWeighmentService { ... }
    //    uses System.Data.SqlClient or MySqlConnector
    // ═══════════════════════════════════════════════════════════
    public static class ServiceLocator
    {
        // ── Data services ─────────────────────────────────────
        public static IWeighmentService WeighmentService { get; private set; }
        public static IVGMService VGMService { get; private set; }
        public static IWeighbridgeHardwareService HardwareService { get; private set; }

        // ── Static constructor — runs once at app startup ─────
        static ServiceLocator()
        {
            // ↓ MOCK — replace these with real implementations
            WeighmentService = new MockWeighmentService();
            VGMService = new MockVGMService();
            HardwareService = new MockHardwareService();
        }

        // ── Optional: allow runtime re-registration ───────────
        public static void Register<T>(T instance)
        {
            if (instance is IWeighmentService ws) WeighmentService = ws;
            else if (instance is IVGMService vs) VGMService = vs;
            else if (instance is IWeighbridgeHardwareService hs) HardwareService = hs;
        }
    }
}

