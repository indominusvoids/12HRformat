using _12HRFormat;
using System;
using UnityEngine;

namespace _12HRFormat
{
    public class CompassInfo : XUiController
    {
        public EntityPlayer player;
        public MinEventParams _params;
        private EntityPlayerLocal localPlayer;

        public readonly CachedStringFormatter<int, int, string> timeFormatter =
        new CachedStringFormatter<int, int, string>(
            (_hour, _min, _ampm) => string.Format("{0:00}:{1:00} {2}", _hour, _min, _ampm)
        );

        public readonly CachedStringFormatter<ulong> daytimeFormatter =
            new CachedStringFormatter<ulong>((_worldTime) =>
                ValueDisplayFormatters.WorldTime(_worldTime, Localization.Get("xuiDayTimeLong"))
            );

        public readonly CachedStringFormatterInt dayFormatter = new CachedStringFormatterInt();
        private float updateTime;

        public override void Init()
        {
            base.Init();
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);
            if (this.localPlayer == null)
                this.localPlayer = this.xui.playerUI.entityPlayer;
            if (GameManager.Instance?.World == null || !XUi.IsGameRunning())
                return;

            this.RefreshBindings();
        }

        public override bool GetBindingValue(ref string value, string bindingName)
        {
            EntityPlayerLocal entityPlayer = base.xui.playerUI.entityPlayer;

            switch (bindingName)
            {
                case "compass_language":
                    value = !GamePrefs.GetBool(EnumGamePrefs.OptionsUiCompassUseEnglishCardinalDirections) ? Localization.language : Localization.DefaultLanguage;
                    return true;
                case "compass_rotation":
                    value = !(this.localPlayer != null) || !(this.localPlayer.playerCamera != null) ? "0.0" : this.localPlayer.playerCamera.transform.eulerAngles.y.ToString();
                    return true;
                case "day":
                    value = "0";
                    if (XUi.IsGameRunning())
                    {
                        int days = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
                        value = this.dayFormatter.Format(days);
                    }
                    return true;
                case "daycolor":
                    value = "FFFFFF";
                    if (XUi.IsGameRunning())
                    {
                        long worldTime = (long)GameManager.Instance.World.worldTime;
                        int num = GameStats.GetInt(EnumGameStats.BloodMoonWarning);
                        (int Days, int Hours, int _) = GameUtils.WorldTimeToElements((ulong)worldTime);
                        if (num != -1 && GameStats.GetInt(EnumGameStats.BloodMoonDay) == Days && num <= Hours)
                            value = "FF0000";
                    }
                    return true;
                case "daytime":
                    value = "";
                    if (XUi.IsGameRunning())
                        value = this.daytimeFormatter.Format(GameManager.Instance.World.worldTime);
                    return true;
                case "daytitle":
                    value = Localization.Get("xuiDay");
                    return true;
                case "showtime":
                    value = (localPlayer == null) ? "true" : (EffectManager.GetValue(PassiveEffects.NoTimeDisplay, _entity: localPlayer) == 0.0).ToString();
                    return true;
                case "time":
                    value = "";
                    if (XUi.IsGameRunning())
                    {
                        (int _, int hours12, int minutes, string ampm) = GameUtilsPatch.WorldTimeToElements(GameManager.Instance.World.worldTime);
                        value = this.timeFormatter.Format(hours12, minutes, ampm);
                    }
                    return true;
                case "timetitle":
                    value = Localization.Get("xuiTime");
                    return true;
                default:
                    return base.GetBindingValue(ref value, bindingName);
            }
        }

        public override void OnOpen()
        {
            base.OnOpen();
            this.xui.playerUI.windowManager.CloseIfOpen("windowpaging");
        }
    }
}
