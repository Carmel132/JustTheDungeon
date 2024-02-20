using UnityEngine;
namespace Christian
{
    public class ChristianAbility1ProjController : MonoBehaviour
    {
        public float maxRadius;
        public float dR = 1 / 20f;
        public float t = 0;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            t += Time.deltaTime;
            if (t * dR >= 1) { Destroy(gameObject); }
            transform.localScale = Vector3.one * Mathf.Lerp(1, maxRadius, dR * t);
        }
    }
}