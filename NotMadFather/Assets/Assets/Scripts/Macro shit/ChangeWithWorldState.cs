using System;
using UnityEngine;

public class WorldControl : MonoBehaviour
{
    private enum ActiveWhen
    {
        Medication,
        Withdrawl
    }

    [SerializeField] ActiveWhen activeWhen;

    void Update()
    {
        if (activeWhen == ActiveWhen.Medication)
        {
            if (Manager.Instance.medication)
            {
                Change(true);
            }
            else
            {
                Change(false);
            }
        }
        else if (activeWhen == ActiveWhen.Withdrawl)
        {
            if (Manager.Instance.medication)
            {
                Change(false);
            }
            else
            {
                Change(true);
            }
        }
    }

    private void Change(bool change)
    {
        Component[] components = GetComponents<Component>();

        foreach (Component comp in components)
        {
            if (comp == this)
                continue;

            if (comp is Behaviour behaviour)
            {
                behaviour.enabled = change;
            }

            else if (comp is Collider collider)
            {
                collider.enabled = change;
            }

            if (comp is SpriteRenderer sr)
            {
                sr.enabled = change;
            }
        }
    }
}
