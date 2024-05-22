using GenericModConfigMenu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace TinkersValley;



internal sealed class TiV : Mod
{

    private ModConfig _config;



    /*********
        ** Public methods
        *********/
    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="helper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper helper)
    {
        this._config = this.Helper.ReadConfig<ModConfig>();

        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        if (this._config.EnableTiv==true)
        {
            helper.Events.Input.ButtonPressed += OnButtonPressed;
        }
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
        // ignore if player hasn't loaded a save yet
        if (!Context.IsWorldReady)
            return;

        // print button presses to the console window
        Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
    }
    
    private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
    {
        // get Generic Mod Config Menu's API (if it's installed)
        var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (configMenu is null)
            return;

        // register mod
        configMenu.Register(
            mod: this.ModManifest,
            reset: () => this._config = new ModConfig(),
            save: () => this.Helper.WriteConfig(this._config)
        );

        // add some config options
        configMenu.AddBoolOption(
            mod: this.ModManifest,
            name: () => this.Helper.Translation.Get("config.enabletiv"),
            tooltip: () => this.Helper.Translation.Get("config.enabletivtip"),
            getValue: () => this._config.EnableTiv,
            setValue: value => this._config.EnableTiv = value
        );
        // configMenu.AddTextOption(
        //     mod: this.ModManifest,
        //     name: () => "Example string",
        //     getValue: () => this._config.ExampleString,
        //     setValue: value => this._config.ExampleString = value
        // );
        // configMenu.AddTextOption(
        //     mod: this.ModManifest,
        //     name: () => "Example dropdown",
        //     getValue: () => this._config.ExampleDropdown,
        //     setValue: value => this._config.ExampleDropdown = value,
        //     allowedValues: new string[] { "choice A", "choice B", "choice C" }
        // );
    }
}