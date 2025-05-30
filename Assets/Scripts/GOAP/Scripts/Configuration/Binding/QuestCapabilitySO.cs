using UnityEngine;
using GOAP.Scripts.Configuration.Binding;
using GOAP.Scripts.Configuration.Capabilities;

[CreateAssetMenu(fileName = "QuestCapability", menuName = "GOAP/Capabilities/Quest")]
public class QuestCapabilitySO : CapabilitySO
{
    public override ICapabilityConfig GetConfig()
    {
        return new QuestCapabilityConfig();
    }
}