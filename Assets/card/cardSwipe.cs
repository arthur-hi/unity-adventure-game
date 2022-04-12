using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class cardSwipe : MonoBehaviour
{
    bool canSwipe = false;
    bool Swiping = false;
    private Animator CardAnim;
    public GameObject WorldUI;
    private Animator UIanim;
    public TextMeshProUGUI tmp;
    private string currentCard = "Menu";

    GameObject SwipeUI;
    GameObject ClickUI;
    
    AudioSource audioData;

    [Serializable]
    public class CardClass {
        //  UI Cards
        public string menu, options;
    }
    CardClass cards = new CardClass();
    

    [Serializable]
    public class PathCards {
        public string start, left, right;
    }
    public PathCards p1c1, p1c2, p1c3, p1c4 = new PathCards();
    public PathCards p2c1, p2c2, p2c3 = new PathCards();
    public PathCards p3c1, p3c2 = new PathCards();
    public PathCards p4c1 = new PathCards();

    void Start()
    {
        CardAnim = gameObject.GetComponent<Animator>();
        UIanim = WorldUI.GetComponent<Animator>();
        SwipeUI = WorldUI.transform.Find("Swipe UI").gameObject;
        ClickUI = WorldUI.transform.Find("Click UI").gameObject;

        SwipeUI.SetActive(false);
        ClickUI.SetActive(false);

        StartCoroutine(Intro());
        
        audioData = GetComponent<AudioSource>();
        audioData.volume=.25f;
            
        cards.menu = "🕹\n\nAdventure 2022\n\ntap play\nto begin";
        cards.options = "🪛\n\nOptions\n\n";

        p1c1.left = "🔮\n\nYou have chosen the path of the Astrologer\n\n";
        p1c1.right = "⚔\n\nYou have chosen the path of the Warrior\n\n";

        p1c2.start = "🍃\n\nAs all journeys begin you are presented with two paths.\n\nDo you take the path on your left on your right?";
        p1c2.left = "🌳\n\nThe path leads you towards a dark forest\n\n";
        p1c2.right = "🧟\n\nThe path leads you towards The Crypt.\n\n";

        p1c3.start = "🌳\n\nYou arrive and spot a distant castle.\n\nDo you travel towards it?";
        p1c3.left = "🏰\n\nYou travel towards the castle.\n\n"; // undead (not zombies)
        p1c3.right = "🌳\n\nYou find yourself travelling down a long road.\n\n"; // goblins

        p2c1.start = p1c2.start;
        p2c1.left = "🧟\n\nThe path leads you towards The Crypt.\n\n";

        p2c2.start = "🧟\n\nYou arrive at The Crypt and find a chest.\n\nDo you open it?";
        p2c2.left = "🧟\n\nYou decide to leave the chest as it was.\n\n";
        p2c2.right = "⚔\n\nYou open the chest and find a sword.\n\n";

        p3c1.start = "🧟\n\n(Sound) has awoken the undead of the crypt.\n\nDo you defend yourself?";
        p3c1.left = "💀\n\nYou attempted to run but the undead were too fast...\n\n";
        p3c1.right = "💀\n\nYou attempted to defend yourself but you were unarmed...\n\n";

        p4c1.start = "🧟\n\nThe sounds from the chest have awoken the undead of the crypt.\n\nDo you defend yourself?";
        p4c1.left = "💀\n\nYou attempted to run but the undead were too fast...\n\n";
        p4c1.right = "🧟\n\nUsing the sword you found you successfully defended yourself.\n\n";
    }
    
    void Update()
    {
        float touch;
        Vector3 mouse = Input.mousePosition;
        touch = mouse.x - (Screen.width/2);
        
        if(SwipeUI.activeSelf && canSwipe) {
            if(Swiping&&Input.GetMouseButton(0)&&touch>=(Screen.width/3)){
                Swiping=false;
                CardAnim.SetTrigger("Right");
                UIanim.SetTrigger("Off");
                StartCoroutine(UpdateCard("right"));
            }
            else if (Swiping&&Input.GetMouseButton(0)&&touch<=-(Screen.width/3)) {
                Swiping=false;
                CardAnim.SetTrigger("Left");
                UIanim.SetTrigger("Off");
                StartCoroutine(UpdateCard("left"));
            }
            if(Swiping&&!Input.GetMouseButton(0)) {
                Swiping=false;
                CardAnim.SetTrigger("Drop");
                UIanim.SetTrigger("Off");
            }
            else if (Input.GetMouseButtonDown(0)){ 
                RaycastHit hit; 
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
                if (Physics.Raycast (ray,out hit,100f)) {
                    Swiping=true;
                    CardAnim.SetTrigger("Tap");
                    UIanim.SetTrigger("On");
                    UIanim.SetTrigger("TextOn");
                }
            }
        }
        else if (ClickUI.activeSelf && canSwipe) {
            if (Input.GetMouseButtonDown(0)){ 
                RaycastHit hit; 
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
                if (Physics.Raycast (ray,out hit,100f)) {
                    
                    switch (hit.collider.gameObject.name)
                    {
                        case "false":
                            //UIanim.SetTrigger("ClickCross");
                            CardAnim.SetTrigger("Left");
                            StartCoroutine(UpdateCard("left"));
                        break;  

                        case "true":
                            //UIanim.SetTrigger("ClickTick");
                            CardAnim.SetTrigger("Right");
                            StartCoroutine(UpdateCard("right"));
                        break;
                    }

                }
            }
        }
    }
    IEnumerator Intro()
    {
        tmp.text="👤\n\na game by arthur\n\n";
        yield return new WaitForSeconds(1);
        CardAnim.SetTrigger("Left");
        StartCoroutine(UpdateCard("start"));
        yield return new WaitForSeconds(2);
        ClickUI.SetActive(true);
        UIanim.SetTrigger("On");
        UIanim.SetTrigger("TextOn");
        canSwipe=true;
    }
    bool showText = false;
    bool showUI = false;
    string rightText = "play";
    string leftText = "options";
    string actionText = "";
    string crossText = "🪛";
    string tickText = "🕹";
    Color White=new Color(.8f, .8f, .79f, .5f);
    Color Red=new Color(0.8196079f,0.2039216f,0.2196078f,1f);
    Color Green=new Color(0f,0.8f,0.4156863f,1f);
    Color Magic=new Color(.55f,.54f,.84f,1f);
    Color Blue=new Color(0f,.6f,.73f,1f);
    Color crossColour;
    Color tickColour;
    bool delayedStart = false;
    IEnumerator UpdateCard(string swipe)
    {
        var newText="";

        TextMeshProUGUI swipeCross=SwipeUI.transform.Find("Cross").gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI swipeTick=SwipeUI.transform.Find("Tick").gameObject.GetComponent<TextMeshProUGUI>();
        
        TextMeshProUGUI clickCross=ClickUI.transform.Find("Cross").gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI clickTick=ClickUI.transform.Find("Tick").gameObject.GetComponent<TextMeshProUGUI>();
        
        TextMeshProUGUI LeftText=WorldUI.transform.Find("Left Text").gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI RightText=WorldUI.transform.Find("Right Text").gameObject.GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI ActionText=WorldUI.transform.Find("Action Text").gameObject.GetComponent<TextMeshProUGUI>();

        switch (currentCard)
        {

            case "Menu":
                
                switch (swipe)
                {
                    case "start":
                        
                        crossColour=White;
                        tickColour=White;
                        
                        newText=cards.menu;
                        leftText="options";
                        rightText="play";
                        
                        break;
                        
                    case "left":

                        UIanim.SetTrigger("Off");
                        currentCard="Options";

                        crossText="";
                        tickText="👉";
                        newText=cards.options;
                        showUI=true;
                        showText=true;
                        leftText="";
                        rightText="back";

                    break;
                    case "right":

                        UIanim.SetTrigger("Off");
                        currentCard="P1C1";
                        crossColour=Magic;
                        tickColour=Blue;

                        crossText="🔮";
                        tickText="⚔️";
                        newText="👥\n\nChoose who you are\n\n";
                        showUI=true;
                        showText=true;
                        leftText="astrologer";
                        rightText="warrior";
                        
                    break;
                }

            break;
            case "Options":
                
                switch (swipe)
                {
                    case "right":

                        UIanim.SetTrigger("Off");
                        currentCard="Menu";
                        showUI=true;

                        newText=cards.menu;
                        crossText="🪛";
                        tickText="🕹";
                        leftText="options";
                        rightText="play";
                        
                    break;
                }

            break;
            case "P1C1":
                
                switch (swipe)
                {
                    case "left":

                        UIanim.SetTrigger("Off");
                        currentCard="P1C2";

                        newText=p1c1.left;

                        crossText="";
                        tickText="";
                        leftText="";
                        rightText="";

                        delayedStart=true;
                        
                        //crossText="❌";
                        //tickText="✔️";
                        
                    break;
                    case "right":

                        UIanim.SetTrigger("Off");
                        currentCard="P2C1";

                        newText=p1c1.right;

                        crossText="";
                        tickText="";
                        leftText="";
                        rightText="";

                        delayedStart=true;
                        
                    break;
                }

            break;
            case "P1C2":
                
                switch (swipe)
                {
                    case "start":
                                    
                        SwipeUI.SetActive(true);
                        ClickUI.SetActive(false);

                        UIanim.SetTrigger("Off");

                        newText=p1c2.start;
                        actionText="try swiping instead";

                        crossColour=White;
                        tickColour=White;

                        crossText="👈";
                        tickText="👉";
                        leftText="left";
                        rightText="right";
                        
                    break;
                    case "left":

                        UIanim.SetTrigger("Off");
                        currentCard="P1C3";

                        newText=p1c2.left;

                        crossText="";
                        tickText="";
                        leftText="";
                        rightText="";

                        delayedStart=true;

                    break;
                    case "right":

                        UIanim.SetTrigger("Off");
                        currentCard="P2C2";

                        newText=p1c2.right;

                        crossText="";
                        tickText="";
                        leftText="";
                        rightText="";

                        delayedStart=true;

                    break;
                }

            break;
            case "P1C3":
                
                switch (swipe)
                {
                    case "start":

                        UIanim.SetTrigger("Off");
                        showText=true;

                        newText=p1c3.start;
                        actionText="";

                        crossColour=Red;
                        tickColour=Green;
                        crossText="❌";
                        tickText="✔️";
                        leftText="No";
                        rightText="Yes";
                        
                    break;
                    case "left":

                        UIanim.SetTrigger("Off");
                        currentCard="P2C2";

                        newText=p1c3.left;

                        crossText="";
                        tickText="";
                        leftText="";
                        rightText="";

                        delayedStart=true;

                    break;
                    case "right":

                        UIanim.SetTrigger("Off");
                        currentCard="P2C2";

                        newText=p1c3.right;

                        crossText="";
                        tickText="";
                        leftText="";
                        rightText="";

                        delayedStart=true;

                    break;
                }

            break;

            case "P2C1":

                switch (swipe)
                {
                    case "start":
                                    
                        SwipeUI.SetActive(true);
                        ClickUI.SetActive(false);
                        actionText="try swiping";

                        UIanim.SetTrigger("Off");

                        newText=p2c1.start;

                        crossColour=White;
                        tickColour=White;

                        crossText="👈";
                        tickText="👉";
                        leftText="left";
                        rightText="right";
                        
                    break;
                    case "left":

                        UIanim.SetTrigger("Off");
                        currentCard="P2C2";

                        newText=p2c1.left;

                        crossText="";
                        tickText="";
                        leftText="";
                        rightText="";

                        delayedStart=true;

                    break;
                    case "right":

                        //  TEMP

                        UIanim.SetTrigger("Off");
                        currentCard="P2C2";

                        newText=p2c1.right;

                        crossText="";
                        tickText="";
                        leftText="";
                        rightText="";

                        delayedStart=true;

                    break;
                }
            
            break;
            case "P2C2":
                
                switch (swipe)
                {
                    case "start":
                                    
                        SwipeUI.SetActive(true);
                        ClickUI.SetActive(false);

                        UIanim.SetTrigger("Off");

                        newText=p2c2.start;
                        actionText="";

                        crossColour=Red;
                        tickColour=Green;
                        crossText="❌";
                        tickText="✔️";
                        
                    break;
                    case "left":
                                    
                        SwipeUI.SetActive(true);
                        ClickUI.SetActive(false);
                        currentCard="P3C1";

                        UIanim.SetTrigger("Off");

                        newText=p2c2.left;
                        actionText="";

                        crossColour=Red;
                        tickColour=Green;
                        crossText="❌";
                        tickText="✔️";
                    

                    break;
                    case "right":
                                    
                        SwipeUI.SetActive(true);
                        ClickUI.SetActive(false);
                        currentCard="P4C1";

                        UIanim.SetTrigger("Off");

                        newText=p2c2.right;
                        actionText="";

                        crossColour=Red;
                        tickColour=Green;
                        crossText="❌";
                        tickText="✔️";


                    break;
                }

            break;

        }

        yield return new WaitForSeconds(.15f);
        audioData.Play(0);
        yield return new WaitForSeconds(.15f);

        tmp.text=newText;
        LeftText.text=leftText;
        RightText.text=rightText;

        clickCross.text=crossText;
        clickTick.text=tickText;
        clickCross.color=crossColour;
        clickTick.color=tickColour;
        
        swipeCross.text=crossText;
        swipeTick.text=tickText;
        swipeCross.color=crossColour;
        swipeTick.color=tickColour;

        ActionText.text=actionText;

        if(showUI) {
            UIanim.SetTrigger("On");
            showUI=false;
        }
        if(showText) UIanim.SetTrigger("TextOn");
        if(delayedStart) {
            delayedStart=false;
            canSwipe=false;
            yield return new WaitForSeconds(3);
            CardAnim.SetTrigger("Left");
            StartCoroutine(UpdateCard("start"));
            yield return new WaitForSeconds(.75f);
            canSwipe=true;
        }
    }
}