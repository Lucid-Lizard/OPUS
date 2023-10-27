using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverylayScript : MonoBehaviour
{
    
    public GameObject PlayerRoot;
    public GameObject PlayerHead;
    public GameObject PlayerArm1;
    public GameObject PlayerArm2;
    public GameObject PlayerLeg1;
    public GameObject PlayerLeg2;

    
    public GameObject OverlayRoot;
    public GameObject OverlayHead;
    public GameObject OverlayArm1;
    public GameObject OverlayArm2;
    public GameObject OverlayLeg1;
    public GameObject OverlayLeg2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OverlayRoot.transform.position = PlayerRoot.transform.position;
        OverlayRoot.transform.rotation = PlayerRoot.transform.rotation;
        OverlayRoot.transform.localScale = PlayerRoot.transform.localScale;

        OverlayHead.transform.position = PlayerHead.transform.position;
        OverlayHead.transform.rotation = PlayerHead.transform.rotation;
        OverlayHead.transform.localScale = PlayerHead.transform.localScale;

        OverlayArm1.transform.position = PlayerArm1.transform.position;
        OverlayArm1.transform.rotation = PlayerArm1.transform.rotation;
        OverlayArm1.transform.localScale = PlayerArm1.transform.localScale;

        OverlayArm2.transform.position = PlayerArm2.transform.position;
        OverlayArm2.transform.rotation = PlayerArm2.transform.rotation;
        OverlayArm2.transform.localScale = PlayerArm2.transform.localScale;

        OverlayLeg1.transform.position = PlayerLeg1.transform.position;
        OverlayLeg1.transform.rotation = PlayerLeg1.transform.rotation;
        OverlayLeg1.transform.localScale = PlayerLeg1.transform.localScale;

        OverlayLeg2.transform.position = PlayerLeg2.transform.position;
        OverlayLeg2.transform.rotation = PlayerLeg2.transform.rotation;
        OverlayLeg2.transform.localScale = PlayerLeg2.transform.localScale;
    }
}
