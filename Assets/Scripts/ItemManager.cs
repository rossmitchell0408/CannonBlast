using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    /****************Singleton*****************/
    public static ItemManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    /*******************************************/

    [SerializeField]
    MissileController missilePrefab;
    [SerializeField]
    Queue<MissileController> missiles;

    // Start is called before the first frame update
    void Start()
    {
        CreateMissiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateMissiles()
    {
        if (missiles == null)
        {
            missiles = new Queue<MissileController>();
        }

        for (int i = 0; i < 10; i++)
        {
            MissileController missile = Instantiate(missilePrefab, transform);
            missile.gameObject.SetActive(false);
            missiles.Enqueue(missile);
        }
    }

    public MissileController GetMissile()
    {
        if (missiles.Count <= 0)
        {
            CreateMissiles();
        }

        return missiles.Dequeue();
    }

    public void ReturnMissile(MissileController missile)
    {
        if (missiles == null)
        {
            CreateMissiles();
        }

        missiles.Enqueue(missile);
        missile.gameObject.SetActive(false);
    }
}
