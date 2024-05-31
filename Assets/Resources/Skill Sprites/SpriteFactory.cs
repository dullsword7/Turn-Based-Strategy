using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFactory : MonoBehaviour
{
    public static SpriteFactory Instance;


    GameObject attackAnimation;
    GameObject slashAnimation;
    // Start is called before the first frame update
    void Start()
    {
        attackAnimation = Resources.Load<GameObject>("Skill Sprites/Attack/AttackAnimation");
        slashAnimation = Resources.Load<GameObject>("Skill Sprites/Slash/SlashAnimation");
    }

    public void InstantiateSkillSprite(string spriteName, Vector3 position)
    {
        Vector3 newPos = new Vector3(position.x, position.y, position.z + 1);

        if (spriteName == "Attack")
        {
            var x = Instantiate(attackAnimation, position, Quaternion.identity);
        }
        if (spriteName == "Slash")
        {
            var x = Instantiate(slashAnimation, position, Quaternion.identity);
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
