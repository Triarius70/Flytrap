using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.rmc.projects.event_dispatcher;
using System;

public class AssetManager : EventDispatcherBase
{
    public List<Transform> AllCreatureTransforms;
    public Transform[] CreatureTransforms;
    public int CreaturesNum = 10;
    protected List<Creature> Creatures;
    protected bool CreatureBonusEffectOn = false;
    public static AssetManager Instance;

	// Use this for initialization
	void Start () {
        Instance = this;
        
        //Invoke("StartGame", 1f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //PUBLIC METHODS
    public void StartGame()
    {
        CreatureBonusEffectOn = false;
        ClearStage();
        CreateCreatures();
    }

    public void CheckMatches(List<Creature> creaturesCaptured)
    {
        bool HasSpecial = false;
        int l = creaturesCaptured.Count;
        if (l > 1)
        {
            
            //Debug.Log( l + " creatures trapped");
            Creature SampleCreature = creaturesCaptured[0];
            int id = SampleCreature.creatureID;

                Vector3 newPosition = Vector3.zero;
                float newScale = 0.0f;

                for (int i = 0; i < l; i++)
                {
                    Creature creature = creaturesCaptured[i];

                    if (creature.HasSpecial)
                        HasSpecial = true;

                    newPosition += creature.gameObject.transform.position;
                    newScale += creature.scale;
                    int index = Creatures.FindIndex(o => o == creature);
                    creature.Die();
                    Creatures.RemoveAt(index);  
                }

                newPosition /= creaturesCaptured.Count;
                newScale *= 0.6f;
                //Debug.Log("newScale:" + newScale);
                NewCreature(newPosition, id, newScale);
                eventDispatcher.dispatchEvent(new CaptureEvent(CaptureEvent.ON_CAPTURE_SUCCESS,l, newPosition, newScale, HasSpecial,  SampleCreature.type));
        }
        else
        {
            eventDispatcher.dispatchEvent(new CaptureEvent(CaptureEvent.ON_CAPTURE_FAIL));
        }

        CheckLevelComplete();
    }

    protected void CheckLevelComplete()
    {
     if(isLastCreature())
        {
            eventDispatcher.dispatchEvent(new GameEvent(GameEvent.ON_LEVEL_COMPLETE));
        }
    }



    protected void CreateCreatures()
    {
        float w = Screen.width;
        float h = Screen.height;

        Creatures = new List<Creature>();

        for (int i = 0; i < CreatureTransforms.Length; i++)
        {
            for (int j = 0; j < CreaturesNum; j++)
            {
                Vector3 pos = new Vector3(UnityEngine.Random.Range(0, w), UnityEngine.Random.Range(0, h), 0f);
                pos = Camera.main.ScreenToWorldPoint(pos);
                NewCreature(pos, i);
            }
        }
    }

    protected void ClearStage()
    {
        if(Creatures != null )
        {
            foreach (Creature item in Creatures)
            {
                item.Die();
            }
        }
    }

    protected void NewCreature(Vector3 pos, int creatureID, float scale = 4.0f)
    {
        Transform tempTransform = F3DPool.instance.Spawn(CreatureTransforms[creatureID], new Vector3(pos.x, pos.y, 0f), Quaternion.identity, null);
        tempTransform.localScale = new Vector2(scale, scale);
        GameObject GO = tempTransform.gameObject;
        Creature creature = GO.GetComponent<Creature>();
        creature.creatureID = creatureID;
        creature.scale = scale;
        Creatures.Add(creature);
        //if scale is bigger its not at the start of the game
        //lets see if we have bonus effect on creature
        if ( scale > 4.0f && !CreatureBonusEffectOn && !isLastCreatureOfType(creature.type) )
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            if(rand > creature.SpecialOdds)
            {
                creature.Special();
                creature.eventDispatcher.addEventListener(CreatureEvent.ON_SPECIAL_COMPLETE, RemoveSpecial);
                eventDispatcher.dispatchEvent( new CreatureEvent( CreatureEvent.ON_SPECIAL_START ));
                CreatureBonusEffectOn = true;
            }
        }
        //return creature;
    }

    private void RemoveSpecial(IEvent iEvent)
    {
        Invoke("SetCreatureBonusEffectBool", 1.0f);
        Creature target = iEvent.target as Creature;
        target.eventDispatcher.removeAllEventListeners();
        eventDispatcher.dispatchEvent(iEvent);
    }

    private void SetCreatureBonusEffectBool()
    {
        CreatureBonusEffectOn = false;
    }

    private bool isLastCreature()
    {
        bool isLast = false;
        if (Creatures.Count <= CreatureTransforms.Length)
        {
            isLast = true;
        }
        return isLast;
    }

    private bool isLastCreatureOfType(string type)
    {
        int numOfCreaturesOfType = 0;
        foreach (Creature c in Creatures)
        {
            if (c.type == type)
                numOfCreaturesOfType++;
        }

        if (numOfCreaturesOfType < 2)
            return true;

        return false;
    }
}
