using Jan.UI;

namespace Jan.Dialogue
{
    public static class DialogueManager
    {
        public static Dialogue<T> GetDialogueById<T>(this Dialogue<T>[] dialogues, T id)
        {
            foreach (var dialogue in dialogues)
            {
                if (dialogue.Id.Equals(id))
                {
                    return dialogue;
                }
            }

            return null;
        }

        public static void EndDialogue()
        {
            if(UIBusManager.TryGetUIElement(out IDialogueUI dialogueUI))
            {
                dialogueUI.Show(false);
            }
        }
    }
}