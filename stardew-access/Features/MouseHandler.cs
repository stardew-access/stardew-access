﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using xTile;
using StardewValley;
using StardewValley.Menus;
using stardew_access.Features;

namespace stardew_access.Features
{
    public class MouseHandler
    {

        private Vector2 ViewingOffset = Vector2.Zero;
        private Vector2 relativeOffsetLockPosition = Vector2.Zero;
        private Boolean relativeOffsetLock = false;
        private Vector2 prevPlayerPosition = Vector2.Zero, prevFacing = Vector2.Zero;

        private Vector2 PlayerFacingVector
        {
            get
            {
                switch (Game1.player.FacingDirection)
                {
                    case 0:
                        return new Vector2(0, -Game1.tileSize);
                    case 1:
                        return new Vector2(Game1.tileSize, 0);
                    case 2:
                        return new Vector2(0, Game1.tileSize);
                    case 3:
                        return new Vector2(-Game1.tileSize, 0);
                    default:
                        return Vector2.Zero;
                }
            }
        }

        private Vector2 PlayerPosition
        {
            get
            {
                int x = Game1.player.GetBoundingBox().Center.X;
                int y = Game1.player.GetBoundingBox().Center.Y;
                return new Vector2(x, y);
            }
        }

        private Vector2 getTileCursorPosition()
        {
            Vector2 target = this.PlayerPosition;
            if (this.relativeOffsetLock)
            {
                target += this.relativeOffsetLockPosition;
            }
            else
            {
                target += this.PlayerFacingVector + this.ViewingOffset;
            }
            return target;
        }

        public void HandleInput()
        {
            if (MainClass.Config.ToggleRelativeCursorLockKey.JustPressed())
            {
                this.relativeOffsetLock = !this.relativeOffsetLock;
                if (this.relativeOffsetLock)
                {
                    this.relativeOffsetLockPosition = this.PlayerFacingVector + this.ViewingOffset;
                } else { 
                    this.relativeOffsetLockPosition = Vector2.Zero;
                }
                MainClass.ScreenReader.Say("Relative cursor lock " + (this.relativeOffsetLock ? "enabled" : "disabled") + ".", true);
            }
                    else if (MainClass.Config.TileCursorPreciseUpKey.JustPressed())
            {
                this.cursorMoveInput(new Vector2(0, -MainClass.Config.TileCursorPreciseMovementDistance), true);
            }
            else if (MainClass.Config.TileCursorPreciseRightKey.JustPressed())
            {
                this.cursorMoveInput(new Vector2(MainClass.Config.TileCursorPreciseMovementDistance, 0), true);
            }
            else if (MainClass.Config.TileCursorPreciseDownKey.JustPressed())
            {
                this.cursorMoveInput(new Vector2(0, MainClass.Config.TileCursorPreciseMovementDistance), true);
            }
            else if (MainClass.Config.TileCursorPreciseLeftKey.JustPressed())
            {
                this.cursorMoveInput(new Vector2(-MainClass.Config.TileCursorPreciseMovementDistance, 0), true);
            }
            else if (MainClass.Config.TileCursorUpKey.JustPressed())
            {
                this.cursorMoveInput(new Vector2(0, -Game1.tileSize));
            }
            else if (MainClass.Config.TileCursorRightKey.JustPressed())
            {
                this.cursorMoveInput(new Vector2(Game1.tileSize, 0));
            }
            else if (MainClass.Config.TileCursorDownKey.JustPressed())
            {
                this.cursorMoveInput(new Vector2(0, Game1.tileSize));
            }
            else if (MainClass.Config.TileCursorLeftKey.JustPressed())
            {
                this.cursorMoveInput(new Vector2(-Game1.tileSize, 0));
            }
        }

        private void cursorMoveInput(Vector2 delta, Boolean precise = false)
        {
                        if (!tryMoveTileView(delta)) return;
            Vector2 position = this.getTileCursorPosition();
            Vector2 tile = new Vector2((float)Math.Floor(position.X / Game1.tileSize), (float)Math.Floor(position.Y / Game1.tileSize));
            String ?name = TileInfo.getNameAtTile(tile);
            if (name == null)
            {
                if (TileInfo.isCollidingAtTile((int)tile.X, (int)tile.Y))
                {
                    name = "blocked";
                } else
                {
                    name = "empty";
                }
            }
            if (precise)
            {
                MainClass.ScreenReader.Say($"{name}, {position.X}, {position.Y}", true);
            }
            else
            {
                MainClass.ScreenReader.Say($"{name}, {(int)(position.X / Game1.tileSize)}, {(int)(position.Y / Game1.tileSize)}", true);
            }
        }

        private bool tryMoveTileView(Vector2 delta)
        {
            Vector2 dest = this.getTileCursorPosition() + delta;
            if (!isPositionOnMap(dest)) return false;
            if ((MainClass.Config.LimitTileCursorToScreen && Utility.isOnScreen(dest, 0)) || !MainClass.Config.LimitTileCursorToScreen)
            {
                if (this.relativeOffsetLock)
                    this.relativeOffsetLockPosition += delta;
                else
                    this.ViewingOffset += delta;
                return true;
            }
            return false;
        }

        private void SnapMouseToPlayer()
        {
            Vector2 cursorPosition = this.getTileCursorPosition();
                        if (allowMouseSnap(cursorPosition))
                Game1.setMousePosition((int)cursorPosition.X - Game1.viewport.X, (int)cursorPosition.Y - Game1.viewport.Y);
        }

        public void update()
        {
            //Reset the viewing cursor to the player when they turn or move. This will not reset the locked offset relative cursor position.
            if (this.prevFacing != this.PlayerFacingVector || this.prevPlayerPosition != this.PlayerPosition)
            {
                this.ViewingOffset = Vector2.Zero;
            }
            this.prevFacing = this.PlayerFacingVector;
            this.prevPlayerPosition = this.PlayerPosition;
            if (MainClass.Config.SnapMouse)
                this.SnapMouseToPlayer();
        }

        private static bool allowMouseSnap(Vector2 point)
        {
            if (!Utility.isOnScreen(point, 0)) return false;

            //prevent mousing over the toolbar or any other UI component with the tile cursor
            foreach (IClickableMenu menu in Game1.onScreenMenus)
            {
                if (menu.isWithinBounds((int)point.X - Game1.viewport.X, (int)point.Y - Game1.viewport.Y)) return false;
            }
            return true;
        }

        private static bool isPositionOnMap(Vector2 position)
        {
            Map map = Game1.currentLocation.map;
            if (position.X < 0 || position.X > map.Layers[0].DisplayWidth) return false;
            if (position.Y < 0 || position.Y > map.Layers[0].DisplayHeight) return false;
            return true;
        }
    }
}