using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using stardew_access.Utils;
using StardewValley;
using StardewValley.Menus;

namespace stardew_access.Patches;

internal class TitleMenuPatch : IPatch
{
    private static bool HasSpokenSkipMessage = false;
    private static bool shouldSnapToDefault = false;
    public void Apply(Harmony harmony)
    {
        harmony.Patch(
                original: AccessTools.Method(typeof(TitleMenu), nameof(TitleMenu.draw), [typeof(SpriteBatch)]),
                postfix: new HarmonyMethod(typeof(TitleMenuPatch), nameof(TitleMenuPatch.DrawPatch))
        );
        harmony.Patch(
                original: AccessTools.Method(typeof(TitleMenu), nameof(TitleMenu.ReturnToMainTitleScreen)),
                postfix: new HarmonyMethod(typeof(TitleMenuPatch), nameof(TitleMenuPatch.ReturnToMainTitleScreenPatch))
        );
    }

    private static void DrawPatch(TitleMenu __instance, bool ___isTransitioningButtons)
    {
        try
        {
            if (___isTransitioningButtons)
                return;

            if (shouldSnapToDefault)
            {
                __instance.populateClickableComponentList();
                __instance.snapToDefaultClickableComponent();
                shouldSnapToDefault = false;
            }

            int x = Game1.getMouseX(true), y = Game1.getMouseY(true); // Mouse x and y position
            string translationKey = "";
            object? translationTokens = null;

            if (TitleMenu.subMenu == null)
            {
                if (__instance.titleInPosition && __instance.showButtonsTimer <= 0)
                {
                    MouseUtils.SimulateMouseClicks(
                        (x, y) => __instance.receiveLeftClick(x, y),
                        null
                    );
                }

                if (__instance.muteMusicButton.containsPoint(x, y))
                {
                    translationKey = "menu-title-mute_music_button";
                }
                else if (__instance.aboutButton.containsPoint(x, y))
                {
                    translationKey = "menu-title-about_button";
                }
                else if (__instance.languageButton.containsPoint(x, y))
                {
                    translationKey = "menu-title-language_button";
                }
                else if (__instance.windowedButton.containsPoint(x, y))
                {
                    translationKey = "menu-title-fullscreen_button";
                    translationTokens = new
                    {
                        is_enabled = Game1.isFullscreen ? 1 : 0
                    };
                }
                else if (__instance.fadeFromWhiteTimer <= 0)
                {
                    if (!HasSpokenSkipMessage)
                    {
                        MainClass.ScreenReader.TranslateAndSayWithMenuChecker("menu-title-click_to_skip", true);
                        HasSpokenSkipMessage = true;
                    }
                    if (__instance.titleInPosition && __instance.showButtonsTimer <= 0)
                    {
                        foreach (var button in __instance.buttons)
                        {
                            if (!button.containsPoint(x, y))
                                continue;

                            translationKey = GetTranslationKeyForButton(button.name);
                            break;
                        }
                    }
                }
            }
            else if (TitleMenu.subMenu != null)
            {
                if (__instance.backButton.containsPoint(x, y) && TitleMenu.subMenu is not CharacterCustomization)
                {
                    translationKey = "common-ui-back_button";
                    MouseUtils.SimulateMouseClicks(
                        (_, _) => __instance.backButtonPressed(),
                        null
                    );
                }
            }

            if (!string.IsNullOrEmpty(translationKey))
                MainClass.ScreenReader.TranslateAndSayWithMenuChecker(translationKey, true, translationTokens);

            IClickableMenuPatch.DrawPatch();
        }
        catch (Exception e)
        {
            Log.Error($"An error occurred in title menu patch:\n{e.Message}\n{e.StackTrace}");
        }
    }

    private static string GetTranslationKeyForButton(string buttonName) => buttonName.ToLower() switch
    {
        "new" => "menu-title-new_game_button",
        "co-op" => "menu-title-co_op_button",
        "load" => "menu-title-load_button",
        "exit" => "menu-title-exit_button",
        "invite" => "menu-title-invite_button",
        _ => ""
    };

    private static void ReturnToMainTitleScreenPatch()
    {
        shouldSnapToDefault = true;
        Game1.options.setGamepadMode("force_on");
        Game1.options.snappyMenus = true;
    }
}
