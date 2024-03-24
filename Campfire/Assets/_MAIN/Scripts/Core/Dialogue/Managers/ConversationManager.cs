using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;
        private Coroutine process = null;
        public bool isRunning => process != null;
        private TextArchitect architect = null;
        private bool userPrompt = false;

        public ConversationManager(TextArchitect architect)
        {
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }
       
       private void OnUserPrompt_Next()
       {
            userPrompt = true;
       }
        public void StartConversation(List<string> conversation)
        {
            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
        }

        public void StopConversation()
        {
            if (!isRunning)
                return;

            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
            //Don't display blank lines or run logic on them.
            for (int i = 0; i < conversation.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(conversation[i]))
                    continue;

                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);

                //Display dialogue.
                if (line.hasDialogue)
                    yield return Line_RunDialogue(line);

                //Run any commands
                if (line.hasCommands)
                    yield return Line_RunCommands(line);

                yield return new WaitForSeconds(1);
            }
        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            //Show or hide speaker's name whether if it is present in the line
            if (line.hasSpeaker)
                dialogueSystem.ShowSpeakerName(line.speaker);
            else
                dialogueSystem.HideSpeakerName();

            //Build dialogue
            architect.Build(line.dialogue);

            while (architect.isBuilding)
            {
                if (!architect.hurryUp)
                    architect.hurryUp = true;
                else
                    architect.ForceComplete();

                userPrompt = false;
                
                yield return BuildDialogue(line.dialogue);
            }
        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            Debug.Log(line.commands);
            yield return null;
        }

        IEnumerator BuildDialogue(string dialogue)
        {
            architect.Build(dialogue);

            while (architect.isBuilding)
            {
                if (userPrompt)
                {
                    if (!architect.hurryUp)
                        architect.hurryUp = false;
                    else 
                        architect.ForceComplete();

                    userPrompt = false;
                }
            }
            yield return null;
        }
    }
}