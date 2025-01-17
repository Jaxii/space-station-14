using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using static Content.Shared.AME.SharedAMEControllerComponent;

namespace Content.Client.AME.UI
{
    [GenerateTypedNameReferences]
    public partial class AMEWindow : SS14Window
    {
        public AMEWindow(AMEControllerBoundUserInterface ui)
        {
            RobustXamlLoader.Load(this);
            IoCManager.InjectDependencies(this);

            EjectButton.OnPressed += _ => ui.ButtonPressed(UiButton.Eject);
            ToggleInjection.OnPressed += _ => ui.ButtonPressed(UiButton.ToggleInjection);
            IncreaseFuelButton.OnPressed += _ => ui.ButtonPressed(UiButton.IncreaseFuel);
            DecreaseFuelButton.OnPressed += _ => ui.ButtonPressed(UiButton.DecreaseFuel);
        }

        /// <summary>
        /// Update the UI state when new state data is received from the server.
        /// </summary>
        /// <param name="state">State data sent by the server.</param>
        public void UpdateState(BoundUserInterfaceState state)
        {
            var castState = (AMEControllerBoundUserInterfaceState) state;

            // Disable all buttons if not powered
            if (Contents.Children != null)
            {
                SetButtonDisabledRecursive(Contents, !castState.HasPower);
                EjectButton.Disabled = false;
            }

            if (!castState.HasFuelJar)
            {
                EjectButton.Disabled = true;
                ToggleInjection.Disabled = true;
                FuelAmount.Text = Loc.GetString("ame-window-fuel-not-inserted-text");
            }
            else
            {
                EjectButton.Disabled = false;
                ToggleInjection.Disabled = false;
                FuelAmount.Text = $"{castState.FuelAmount}";
            }

            if (!castState.IsMaster)
            {
                ToggleInjection.Disabled = true;
            }

            if (!castState.Injecting)
            {
                InjectionStatus.Text = Loc.GetString("ame-window-engine-injection-status-not-injecting-label") + " ";
            }
            else
            {
                InjectionStatus.Text = Loc.GetString("ame-window-engine-injection-status-injecting-label") + " ";
            }

            CoreCount.Text = $"{castState.CoreCount}";
            InjectionAmount.Text = $"{castState.InjectionAmount}";
        }
    }
}
