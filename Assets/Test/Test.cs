using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject target;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = target.transform.position - transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TestSpriteFactory.Instance.InstantiateSkillSprite("Slash", target.transform.position, direction);
        }
    }
    private IEnumerator SmoothLerp (float time)
{
        Vector3 startingPos  = transform.position;
        Vector3 finalPos = transform.position + Vector3.up * 5;

        float elapsedTime = 0;
        
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = finalPos;

}
}