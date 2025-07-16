using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftController : MonoBehaviour
{
    [SerializeField] private GameObject _inventaryPool;
    [SerializeField] private CraftSettings settings;
    private List<GameObject> selected = new();
    private List<ICrafteble> items = new();

    private void Start()
    {
        settings.Initialization();
    }

    public void EnterCraftMode()
    {
        selected.Clear();
        items = GetComponentsInChildren<ICrafteble>().ToList();
        Debug.Log(items.Count);
        foreach (var item in items)
        {
            Button button = null;
            if ((bool)((MonoBehaviour)item)?.gameObject.TryGetComponent(out button))
            {
                button.onClick.AddListener(() => { Select(button.gameObject); });
                continue;
            }
            button = ((MonoBehaviour)item)?.gameObject.AddComponent<Button>();
            button.onClick.AddListener(() => { Select(button.gameObject); });
        }
    }

    private void Select(GameObject CurrentObject)
    {
        if (selected.Contains(CurrentObject))
        {
            selected.Remove(CurrentObject);
            CurrentObject.GetComponent<Image>().color = Color.white;
        }
        else
        {
            selected.Add(CurrentObject);

            CurrentObject.GetComponent<Image>().color = new Color(1, .5f, .5f, .7f);
        }
        CheckCombo();
    }

    private void CheckCombo()
    {
        List<string> selectedName = new();
        foreach (var item in selected)
        {
            string name = item.GetComponent<ICrafteble>().NameId;
            selectedName.Add(name);
        }
        selectedName.Sort();
        foreach (var combination in settings.CraftCombinations)
        {
            if (combination.Source.SequenceEqual(selectedName))
            {
                Debug.Log("Math");
                foreach (var item in selected)
                {
                    Destroy(item);
                }
                Instantiate(combination.Result, _inventaryPool.transform, false);
                Refresh();
            }
        }
    }

    private void Refresh()
    {
        EnterCraftMode();
    }
}
