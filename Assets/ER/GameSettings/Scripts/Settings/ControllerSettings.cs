using System.Collections.Generic;

namespace ER.GameSetting
{
    public class ControllerSettings : GameSettingBase<ControllerSettings>
    {

        public override Dictionary<string, string> GetSettingInfo()
        {
            var dic = new Dictionary<string, string>();
            return dic;
        }

        public override void UpdateSettings(Dictionary<string, string> dic)
        {

        }

        public void Set(ControllerConfig config)
        {

        }
    }
}