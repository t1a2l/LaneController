using HarmonyLib;
using LaneController.Manager;

namespace LaneController.Patches
{
    [HarmonyPatch(typeof(NetSegment), "UpdateLanes")]
    public static class NetSegmentUpdateLanesPatch
    {
        public static void Postfix(ushort segmentID)
        {
            LaneControllerManager.Instance?.UpateLanes(segmentID);
        }
    }
}
