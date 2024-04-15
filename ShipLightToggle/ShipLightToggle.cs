using OWML.Common;
using OWML.ModHelper;
using System.Collections.Generic;
using UnityEngine;

namespace ShipLightToggle
{
    public class ShipLightToggle : ModBehaviour
    {
        static readonly string[] SHIP_LIGHT_PATHS = [
            "Module_Cabin/Lights_Cabin/Pointlight_HEA_ShipCabin",
            "Module_Cabin/Lights_Cabin/Pointlight_HEA_ShipCabin_Poster",
            "Module_Cockpit/Lights_Cockpit/Pointlight_HEA_ShipCockpit",
            "Module_Supplies/Lights_Supplies/Pointlight_HEA_ShipSupplies_Top",
            "Module_Supplies/Lights_Supplies/Pointlight_HEA_ShipSupplies_Lower",
            "Module_Supplies/Lights_Supplies/Pointlight_HEA_ShipSupplies_Poster"
        ];

        readonly List<ShipLight> shipLights = [];

        private void Start()
        {
            ModHelper.Console.WriteLine($"{nameof(ShipLightToggle)} is loaded!", MessageType.Success);

            LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
            {
                if (loadScene != OWScene.SolarSystem) return;
                ModHelper.Events.Unity.FireOnNextUpdate(() =>
                {
                    shipLights.Clear();
                    var shipT = Locator.GetShipTransform();
                    foreach (var path in SHIP_LIGHT_PATHS)
                    {
                        var childT = shipT.Find(path);
                        var shipLight = childT.GetComponent<ShipLight>();
                        shipLights.Add(shipLight);
                    }
                    foreach (var light in shipLights)
                    {
                        LightSwitch.Create(light);
                    }
                    var combinedParentT = shipT.Find("Module_Cockpit/Lights_Cockpit");
                    var combinedSwitch = LightSwitch.Create(shipLights, combinedParentT);
                    combinedSwitch.transform.localPosition = new Vector3(-1.5f, 0.4f, 4.6f);
                });
            };
        }
    }

}
