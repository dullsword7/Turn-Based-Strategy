using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFactory : MonoBehaviour
{
    public static SpriteFactory Instance;


    private GameObject attackAnimation;
    private GameObject slashAnimation;
    private GameObject movementPath; 
    // Start is called before the first frame update
    void Start()
    {
        attackAnimation = Resources.Load<GameObject>("Skill Sprites/Attack/AttackAnimation");
        slashAnimation = Resources.Load<GameObject>("Skill Sprites/Slash/SlashAnimation");
        movementPath = Resources.Load<GameObject>("Movement Path");
        movementPath = Resources.Load<GameObject>("AttackRange");
    }

    public void InstantiateSkillSprite(string spriteName, Vector3 position, Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270;

        if (spriteName == "Attack")
        {
            var x = Instantiate(attackAnimation, position, Quaternion.AngleAxis(angle, Vector3.forward));
        }
        if (spriteName == "Slash")
        {
            var x = Instantiate(slashAnimation, position, Quaternion.AngleAxis(angle, Vector3.forward));
        }
        if (spriteName == "Movement Path")
        {
            var x = Instantiate(movementPath, position, Quaternion.identity);
            Destroy(x, 3f);
        }
        if (spriteName == "AttackRange")
        {
            var x = Instantiate(movementPath, position, Quaternion.identity);
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
