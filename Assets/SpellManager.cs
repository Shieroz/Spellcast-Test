using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellManager : MonoBehaviour
{
    public List<GameObject> spellBlocks;
    public Material spellBlock, spellBlockTransparent;
    public Transform rightHand;
    List<GameObject> spell;
    Node fireball;

    void Start()
    {
        fireball = new Node(1, new Node(3, new Node(7, new Node(5, "Fireball!"))));
        spell = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            AddSpell();
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
        {
            Debug.Log("Cast");
            foreach (GameObject g in spell)
            {
                Debug.Log(g.name);
            }
            CastSpell();
            Debug.Log("Done casting");
        }
    }

    void AddSpell()
    {
        RaycastHit hit;
        if (Physics.Raycast(rightHand.transform.position, rightHand.transform.forward, out hit))
        {
            if (hit.transform.CompareTag("Spell") && (spell.Count == 0 || hit.transform.gameObject.GetInstanceID() != spell[spell.Count - 1].GetInstanceID()))
            {
                spell.Add(hit.transform.gameObject);
                hit.transform.gameObject.GetComponent<Renderer>().material = spellBlock;
            }
        }
    }

    void CastSpell()
    {
        CheckSpell(spell, fireball);
        spell = new List<GameObject>();
    }
    void CheckSpell(List<GameObject> list, Node tree)
    {
        if (list.Count == 0)
        {
            return;
        }
        else
        {
            if (tree != null && Int32.Parse(list[0].name) == tree.value)
            {
                list.RemoveAt(0);
                if (list.Count >= 1)
                {
                    foreach(Node n in tree.children)
                    {
                        CheckSpell(list, n);
                    }
                }
                else
                {
                    Debug.Log(tree.spellName);
                }
            }
        }
    }
    
    class Node
    {
        public int value;
        public string spellName;
        public Node[] children;

        public Node(int value, string spellName, params Node[] children)
        {
            this.value = value;
            this.spellName = spellName;
            this.children = children;
        }

        public Node(int value, params Node[] children)
        {
            this.value = value;
            this.spellName = "";
            this.children = children;
        }
    }
}

