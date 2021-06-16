using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Unity.Entities;

public class WaterBossBehaviourTree : MonoBehaviour
{
    public InGameInterfaceScript ingame_interface;
    public static WaterBossBehaviourTree _instance;

    void Awake()
    {

        if (_instance == null)
        {

            _instance = this;
            DontDestroyOnLoad(this.gameObject);


        }
        else
        {
            Destroy(this);
        }

        eManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private EntityManager eManager;

    [SerializeField] public  float rangePlayerBoss;

    [SerializeField] public float cdSpell1;
    [SerializeField] public float cdSpell2;

    [SerializeField] public float timeLeftSpell1;
    [SerializeField] public float timeLeftSpell2;
    public Entity boss;


    private readonly Sequence behaviourTree = new Sequence();

    private states testCaC()
    {

        if (rangePlayerBoss < 10)
        {

            print("le boss est au corps à corps");
            return states.Success;

        }



        print("le boss est à distance");
        return states.Failure;
    }

    private states testcd1()
    {

        if(timeLeftSpell1 <= 0)
        {
            print("testCD1 ok");
            return states.Success;

           
        }

        else
        {
            print("testCD1 KO");
            return states.Failure;
        }

        

    }

    private states testcd2()
    {

        if (timeLeftSpell2 <= 0)
        {
            print("testCD2 ok");
            return states.Success;
        }

        else
        {
            print("testCD2 KO");
            return states.Failure;
        }

    }

    private states castSpell1()
    {

        eManager.AddComponent<Spell1Available>(boss);

        print("Spell 1 casting ok");

        return states.Success;

    }

    private states castSpell2()
    {

        print("spell 2 casting");

        eManager.AddComponent<Spell2Available>(boss);

        return states.Success;

    }

  
    void Start()
    {
        Nodes cac = new Nodes(testCaC, baseNodeType.Condition);
        Nodes spellcd1 = new Nodes(testcd1, baseNodeType.Condition);
        Nodes castspell1 = new Nodes(castSpell1, baseNodeType.Action);

        Sequence spell1Sequence = new Sequence();
        spell1Sequence.AddNode(cac);
        spell1Sequence.AddNode(spellcd1);
        spell1Sequence.AddNode(castspell1);

        Nodes spellcd2 = new Nodes(testcd2, baseNodeType.Condition);
        Nodes castspell2 = new Nodes(castSpell2, baseNodeType.Action);

        Sequence spell2Sequence = new Sequence();
        spell2Sequence.AddNode(spellcd2);
        spell2Sequence.AddNode(castspell2);


        //Nodes orderAction = new Nodes(OrderFood, baseNodeType.Action);

        Selector BossSelector = new Selector();

        BossSelector.AddNode(spell1Sequence);

        BossSelector.AddNode(spell2Sequence);
        //orderSelector.AddNode(orderAction);

        behaviourTree.AddNode(BossSelector);


    }

    private IEnumerator excuteTree()
    {
        yield return new WaitForSeconds(5.5f);

    }

    private void Update()
    {
        if(ingame_interface.gameObject.activeSelf)
        {
            states result = behaviourTree.Execute();
            behaviourTree.Initialize();

            print(" update du tree");
        }
    }
}
