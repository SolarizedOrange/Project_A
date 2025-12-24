using UnityEngine;

public class CoverObject : MonoBehaviour
{
    [SerializeField] CharacterBase coverCharacter;
    [SerializeField] int wallDirX;

    public bool TryEnterCover(CharacterBase character)
    {
        if (coverCharacter != null) return false;
        coverCharacter = character;
        
        return true;
    }

    public void ExitCover(CharacterBase character)
    {
        if (coverCharacter == character)
            coverCharacter = null;
    }
}
