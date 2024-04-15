using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShipLightToggle
{
    public class LightSwitch : MonoBehaviour
    {
        List<ShipLight> lights;
        InteractReceiver receiver;

        public static LightSwitch Create(ShipLight light) => Create([light], light.transform);

        public static LightSwitch Create(List<ShipLight> lights, Transform parent)
        {
            var go = new GameObject("LightSwitch");
            var lightSwitch = go.AddComponent<LightSwitch>();
            lightSwitch.lights = lights;
            go.transform.position = parent.transform.position;
            go.transform.parent = parent.transform.parent;
            return lightSwitch;
        }

        void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("Interactible");
            var col = gameObject.GetAddComponent<SphereCollider>();
            col.radius = 0.4f;
            col.isTrigger = true;
            gameObject.GetAddComponent<OWCollider>();
            receiver = gameObject.GetAddComponent<InteractReceiver>();
            receiver._usableInShip = true;
            receiver._interactRange = 2f;
            receiver.EnableInteraction();
            receiver.ChangePrompt("Toggle Light Switch");
            receiver.OnPressInteract += OnPressInteract;
        }

        void OnDestroy()
        {
            receiver.OnPressInteract -= OnPressInteract;
        }

        void OnPressInteract()
        {
            if (lights != null)
            {
                var isOn = lights.Any(l => l.IsOn());
                if (isOn)
                {
                    foreach (var light in lights)
                        light.SetOn(false);
                    Locator.GetPlayerAudioController().PlayTurnOffFlashlight();
                }
                else
                {
                    foreach (var light in lights)
                        light.SetOn(true);
                    Locator.GetPlayerAudioController().PlayTurnOnFlashlight();
                }
            }
            receiver.ResetInteraction();
        }
    }
}
