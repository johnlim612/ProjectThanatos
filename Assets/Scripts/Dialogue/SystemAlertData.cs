using System.Collections.Generic;

namespace EntityData {
    public class SystemAlertData : EntityData {
        private Queue<(string, string)> _systemAlerts;

        public SystemAlertData(string name, UI.EntityType type, Queue<(string, string)> alerts) {
            Name = name;
            DataType = type;
            _systemAlerts = alerts;
        }

        public Queue<(string, string)> GetSystemAlerts() {
            return _systemAlerts;
        }
    }
}
