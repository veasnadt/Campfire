using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace TESTING {
    public class Testing_Architect : MonoBehaviour
    {
        DialogueSystem ds;
        TextArchitect architect;

        string[] lines = new string[5]
        {
            "My name is Monkey D. Luffy and I'm gonna be king of the pirates!",
            "I don't want to go there. It's scary.",
            "Do you think it's safe to go there? It seems awfully suspicious. I'd rather not die.",
            "You ready to head out? You're gonna be late if you take any longer. We got important things to do.",
            "Where did he end up? It might be a lost cause looking for him. If we don't get anywhere by midnight, let's give up on searching."
        };
        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.instance;
            architect = new TextArchitect(ds.dialogueContainer.dialogueText);
            architect.buildMethod = TextArchitect.BuildMethod.typewriter;
            architect.speed = 0.5f;
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (architect.isBuilding)
                {
                    if (!architect.hurryUp) 
                        architect.hurryUp = true;
                    else
                        architect.ForceComplete();
                }
                else 
                    architect.Build(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}