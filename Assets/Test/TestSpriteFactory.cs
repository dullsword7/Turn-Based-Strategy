using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpriteFactory : MonoBehaviour
{
    public static TestSpriteFactory Instance;


    GameObject attackAnimation;
    GameObject slashAnimation;
    // Start is called before the first frame update
    void Start()
    {
        attackAnimation = Resources.Load<GameObject>("Skill Sprites/Attack/AttackAnimation");
        slashAnimation = Resources.Load<GameObject>("Skill Sprites/Slash/SlashAnimation");
    }

    public void InstantiateSkillSprite(string spriteName, Vector3 position, Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270;

        if (spriteName == "Attack")
        {
            var x = Instantiate(attackAnimation, position, Quaternion.identity);
        }
        if (spriteName == "Slash")
        {
            Debug.Log(direction);
            var x = Instantiate(slashAnimation, position, Quaternion.AngleAxis(angle, Vector3.forward));
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
