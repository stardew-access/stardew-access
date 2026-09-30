using stardew_access.Patches;
using StardewModdingAPI;
using StardewValley;

namespace stardew_access.Utils;

internal static class MouseUtils
{
    private static bool wasRecentlyClicked = false;

    internal static async void SimulateMouseClicksWithDelay(Action<int, int>? leftClickHandler, Action<int, int>? rightClickHandler)
    {
        if (!wasRecentlyClicked && SimulateMouseClicks(leftClickHandler, rightClickHandler))
        {
            wasRecentlyClicked = true;
            await Task.Delay(300);
            wasRecentlyClicked = false;
        }
    }

    internal static bool SimulateMouseClicks(Action<int, int>? leftClickHandler, Action<int, int>? rightClickHandler)
    {
        int mouseX = Game1.getMouseX(true);
        int mouseY = Game1.getMouseY(true);

        if (leftClickHandler != null)
        {
            if (MainClass.Config.LeftClickMainKey.JustPressed())
            {
#if DEBUG
                Log.Debug($"Simulating left mouse click at {mouseX}x {mouseY}y in menu {IClickableMenuPatch.ActiveMenuOrSubMenu}");
#endif
                MainClass.ModHelper!.Input.Press(SButton.MouseLeft);
                leftClickHandler(mouseX, mouseY);
                return true;
            }
            if (MainClass.Config.LeftClickAlternateKey.JustPressed())
            {
#if DEBUG
                Log.Debug($"Simulating left mouse click at {mouseX}x {mouseY}y in menu {IClickableMenuPatch.ActiveMenuOrSubMenu}");
#endif
                leftClickHandler(mouseX, mouseY);
                return true;
            }
        }
        
        if (rightClickHandler != null)
        {
            if (MainClass.Config.RightClickMainKey.JustPressed())
            {
#if DEBUG
                Log.Debug($"Simulating right mouse click at {mouseX}x {mouseY}y");
#endif
                MainClass.ModHelper!.Input.Press(SButton.MouseRight);
                rightClickHandler(mouseX, mouseY);
                return true;
            }

            if (MainClass.Config.RightClickAlternateKey.JustPressed())
            {
#if DEBUG
                Log.Debug($"Simulating right mouse click at {mouseX}x {mouseY}y");
#endif
                rightClickHandler(mouseX, mouseY);
                return true;
            }
        }

        return false;
    }
}
