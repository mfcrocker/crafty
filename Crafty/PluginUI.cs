using ImGuiNET;
using System;
using System.Numerics;
using CraftyPlugin.API;

namespace CraftyPlugin
{
    // It is good to have this be disposable in general, in case you ever need it
    // to do any cleanup
    class PluginUI : IDisposable
    {
        private Configuration configuration;

        // this extra bool exists for ImGui, since you can't ref a property
        private bool visible = false;
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        private bool settingsVisible = false;
        public bool SettingsVisible
        {
            get { return this.settingsVisible; }
            set { this.settingsVisible = value; }
        }

        // passing in the image here just for simplicity
        public PluginUI(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public void Dispose()
        {
        }

        public void Draw()
        {
            // This is our only draw handler attached to UIBuilder, so it needs to be
            // able to draw any windows we might have open.
            // Each method checks its own visibility/state to ensure it only draws when
            // it actually makes sense.
            // There are other ways to do this, but it is generally best to keep the number of
            // draw delegates as low as possible.

            DrawMainWindow();
            DrawSettingsWindow();
        }

        public async void DrawMainWindow()
        {
            if (!Visible)
            {
                return;
            }

            Universalis universalis = new Universalis("Omega", "Chaos");

            ImGui.SetNextWindowSize(new Vector2(500, 500), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(new Vector2(500, 500), new Vector2(float.MaxValue, float.MaxValue));
            if (ImGui.Begin("Crafty", ref this.visible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                ImGui.BeginTable("Table", 5);
                ImGui.TableHeader("Recipe");
                ImGui.TableSetupColumn("Recipe");
                ImGui.NextColumn();
                ImGui.TableHeader("Listings");
                ImGui.TableSetupColumn("Listings");
                ImGui.NextColumn();
                ImGui.TableHeader("Sale Price");
                ImGui.TableSetupColumn("Sale Price");
                ImGui.NextColumn();
                ImGui.TableHeader("Sale Velocity");
                ImGui.TableSetupColumn("Sale Velocity");
                ImGui.NextColumn();
                ImGui.TableHeader("DC Name");
                ImGui.TableSetupColumn("DC Name");
                ImGui.NextColumn();

                ImGui.TableHeadersRow();

                int itemID = 22559; // Bar Stool
                UniversalisItemData itemData = await universalis.GetItemFromDataCenter(itemID);
                ImGui.TableNextRow();
                ImGui.NextColumn();
                ImGui.Text("Bar Stool");
                ImGui.NextColumn();
                ImGui.Text(itemData.items[0].listings.Length.ToString());
                ImGui.NextColumn();
                ImGui.Text(itemData.items[0].minPrice.ToString());
                ImGui.NextColumn();
                ImGui.Text(itemData.items[0].regularSaleVelocity.ToString());
                ImGui.NextColumn();
                ImGui.Text(itemData.dcName);

                ImGui.EndTable();
            }
            ImGui.End();
        }

        public void DrawSettingsWindow()
        {
            if (!SettingsVisible)
            {
                return;
            }

            ImGui.SetNextWindowSize(new Vector2(232, 75), ImGuiCond.Always);
            if (ImGui.Begin("A Wonderful Configuration Window", ref this.settingsVisible,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                // can't ref a property, so use a local copy
                var configValue = this.configuration.SomePropertyToBeSavedAndWithADefault;
                if (ImGui.Checkbox("Random Config Bool", ref configValue))
                {
                    this.configuration.SomePropertyToBeSavedAndWithADefault = configValue;
                    // can save immediately on change, if you don't want to provide a "Save and Close" button
                    this.configuration.Save();
                }
            }
            ImGui.End();
        }
    }
}
