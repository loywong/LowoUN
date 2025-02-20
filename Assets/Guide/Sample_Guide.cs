using LowoUN.Guide;
using LowoUN.Util;
using UnityEngine;

public class Sample_Guide : MonoBehaviour {
    [SerializeField] GameObject m_obj1_ui;
    [SerializeField] GameObject m_obj2_3d;
    [SerializeField] GameObject m_obj3_ui;

    void Start () {
        void Action_CompleteGuide () {
            Log.Green ("guide", "All guide steps has completed!");
        }
        GuideManager.Instance.Init (Action_CompleteGuide);
    }

    void OnGUI () {
        GUI.skin.button.fontSize = 24;

        if (GUI.Button (new Rect (10, 10, 160, 40), "Step 1"))
            GuideManager.Instance.EnterGuide (GuideStep.Step1, false, m_obj1_ui, GuideAreaType.Rect);
        // , string.Empty, () => { GuideManager.Instance.ExitGuide (GuideStep.Step1); });

        if (GUI.Button (new Rect (180, 10, 160, 40), "Step 2"))
            GuideManager.Instance.EnterGuide (GuideStep.Step2, true, m_obj2_3d, GuideAreaType.Circle, "This is a 3D game object on the scene!");

        if (GUI.Button (new Rect (360, 10, 160, 40), "Step 3"))
            GuideManager.Instance.EnterGuide (GuideStep.Step3, false, m_obj3_ui, GuideAreaType.Circle);
            
        // if (GUI.Button (new Rect (540, 10, 160, 40), "Step 4"))
        //     GuideManager.Instance.EnterGuide (GuideStep.Step4, false, null, GuideType.Tips, "Congratulations, you have complete all newer's guide steps!", () => {
        //         GuideManager.Instance.ExitGuide (GuideStep.Step4);
        //     });

        if (GUI.Button (new Rect (10, 60, 160, 40), "Step 1 Out"))
            GuideManager.Instance.ExitGuide (GuideStep.Step1);
        if (GUI.Button (new Rect (180, 60, 160, 40), "Step 2 Out"))
            GuideManager.Instance.ExitGuide (GuideStep.Step2);
        if (GUI.Button (new Rect (360, 60, 160, 40), "Step 3 Out"))
            GuideManager.Instance.ExitGuide (GuideStep.Step3);
    }
}