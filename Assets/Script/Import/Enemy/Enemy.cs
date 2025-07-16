using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _timeWaitRecover;
    [SerializeField] private float _force;
    [SerializeField] private Renderer Renderer;
    private Material Material;
    private Coroutine _coroutine;
    private Transform body;

    private Vector3 _startPos;

    private void Start()
    {
        body = Renderer.gameObject.transform;
        _startPos = body.position;
        Material = Renderer.material;
        Material.color = Color.yellow;
    }

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            if (_coroutine == null)
            {
                Animated();
                health.TakeDamage();
                Material.color = Color.red;

                Vector3 direction = other.transform.position - transform.position;

                other.attachedRigidbody.AddForce((direction.normalized + Vector3.up) * _force * 10f);
                Debug.Log(" Я атакую ");
                _coroutine = StartCoroutine(WaitRecover());
            }
        }
    }

    private void Animated()
    {
        Sequence anim = DOTween.Sequence();

        anim.Append(body.DOScale(new Vector3(3, 3, 3), 0.5f).From(new Vector3(1, 1, 1)).SetLoops(2, LoopType.Yoyo))
            .Join(body.DOMoveY(_startPos.y + 1, 1).From(_startPos.y).SetLoops(2, LoopType.Yoyo));
    }

    private IEnumerator WaitRecover()
    {
        yield return new WaitForSeconds(_timeWaitRecover);
        Material.color = Color.yellow;
        _coroutine = null;
    }
}
