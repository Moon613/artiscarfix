using BepInEx.Logging;
using Menu.Remix.MixedUI;
using Menu.Remix.MixedUI.ValueTypes;
using UnityEngine;

namespace SpriteReplacementCo;
public class SpriteReplacementCoOptions : OptionInterface {
    private readonly ManualLogSource Logger;
    public readonly Configurable<bool> isCatB = new Configurable<bool>(false);
    private UIelement[] UIArrPlayerOptions;

    public SpriteReplacementCoOptions(SpriteReplacementCo pluginInstance, ManualLogSource logSource) {
        Logger = logSource;
        isCatB = config.Bind("isCatB", false, (ConfigurableInfo)null);
    }

    public override void Initialize() {
        OpTab opTab = new OpTab(this, "OptionsLongLegs");
        this.Tabs = new[]
        {
            opTab
        };

        UIArrPlayerOptions = new UIelement[]
        {
            new OpLabel(260f, 570f, "Options", true),
            new OpLabel(70f, 490f, "01101001 01110011 01000011"),
            new OpLabel(210f, 510f, "01100001 01110100"),
            new OpLabel(666f, 399f, "01000010"),

            new OpCheckBox(isCatB, new Vector2(330f,400f)) { description = "-. --- - .... .. -. --. / - --- / .-- --- .-. .-. -.-- / .- -... --- ..- - / ---... -.--.-" },
           
        };
        opTab.AddItems(UIArrPlayerOptions);
    }
}
