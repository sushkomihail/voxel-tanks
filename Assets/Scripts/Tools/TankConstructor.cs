using CustomPhysics;
using Tank;
using Tank.Modules.Engine;
using Tank.Modules.Track;
using UnityEditor;
using UnityEngine;

namespace Tools
{
    public class TankConstructor : EditorWindow
    {
        private const string MenuItemPath = "Tools/";
        private const string MenuItemTitle = "Tank Constructor";

        private string _name;
        private Transform _chassis;
        private Transform _turret;
        private Transform _gun;
        
        [MenuItem(MenuItemPath + MenuItemTitle)]
        private static void ShowConstructionWindow()
        {
            GetWindow<TankConstructor>(MenuItemTitle);
        }

        private void OnGUI()
        {
            _name = EditorGUILayout.TextField("Name", _name);
            _chassis = EditorGUILayout.ObjectField("Chassis", _chassis, typeof(Transform), true) as Transform;
            _turret = EditorGUILayout.ObjectField("Turret", _turret, typeof(Transform), true) as Transform;
            _gun = EditorGUILayout.ObjectField("Gun", _gun, typeof(Transform), true) as Transform;

            if (GUILayout.Button("Construct"))
            {
                Construct();
            }
        }

        private void Construct()
        {
            var root = new GameObject(_name);
            root.AddComponent<PlayerTank>();
            
            // First nesting level
            var cameraTarget = new GameObject("CameraTarget");
            cameraTarget.transform.SetParent(root.transform);
            
            var chassisPivot = new GameObject("ChassisPivot");
            _chassis?.SetParent(chassisPivot.transform);
            chassisPivot.transform.SetParent(root.transform);
            chassisPivot.AddComponent<TankChassis>();
            
            var internalModules = new GameObject("InternalModules");
            internalModules.transform.SetParent(root.transform);
            
            var center = new GameObject("Center");
            center.transform.SetParent(root.transform);
            
            // Second nesting level
            var turretPivot = new GameObject("TurretPivot");
            _turret?.SetParent(turretPivot.transform);
            turretPivot.transform.SetParent(chassisPivot.transform);
            turretPivot.AddComponent<TankTurret>();
            
            var leftTrack = new GameObject("LeftTrack");
            leftTrack.transform.SetParent(chassisPivot.transform);
            leftTrack.AddComponent<Track>();
            
            var rightTrack = new GameObject("RightTrack");
            rightTrack.transform.SetParent(chassisPivot.transform);
            rightTrack.AddComponent<Track>();
            
            var engine = new GameObject("Engine");
            engine.transform.SetParent(internalModules.transform);
            engine.AddComponent<Engine>();
            
            // Third nesting level
            var gunPivot = new GameObject("GunPivot");
            _gun?.SetParent(gunPivot.transform);
            gunPivot.transform.SetParent(turretPivot.transform);
            gunPivot.AddComponent<TankGun>();
            
            var fl = new GameObject("FL");
            fl.transform.SetParent(leftTrack.transform);
            fl.AddComponent<CustomWheelCollider>();
            
            var rl = new GameObject("RL");
            rl.transform.SetParent(leftTrack.transform);
            rl.AddComponent<CustomWheelCollider>();
            
            var fr = new GameObject("FR");
            fr.transform.SetParent(rightTrack.transform);
            fr.AddComponent<CustomWheelCollider>();
            
            var rr = new GameObject("RR");
            rr.transform.SetParent(rightTrack.transform);
            rr.AddComponent<CustomWheelCollider>();
            
            // Fourth nesting level
            var projectilePivot = new GameObject("ProjectilePivot");
            projectilePivot.transform.SetParent(gunPivot.transform);
        }
    }
}
