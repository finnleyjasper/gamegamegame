using UnityEngine;
public interface IDialogueObject
{
    void OnDialogueFinished();

    void Speak();

    string Name();

    void UpdateDialogue(string[] newDialogue);
}
