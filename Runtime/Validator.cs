using System;

namespace UPatterns
{
    public record UValidator(Func<string, bool> validate, string ErrorMessage)
    {
        public static UValidator LengthMin(int length) =>
            new UValidator(value => value.Length >= length, $"Must be {length} characters.");

        public static UValidator LengthEqual(int length) =>
            new UValidator(value => value.Length == length, $"Must be {length} characters.");

        public static UValidator ValueVerify(Func<string> valueVerify) =>
            new UValidator(value => value == valueVerify(), $"Must be value equal to {valueVerify}");

        public static UValidator[] CharacterRegex(
            bool? uppercase = null, 
            bool? lowercase = null, 
            bool? number = null,
            bool? specialCharacter = null) 
        {
            System.Collections.Generic.List<UValidator> lst = new();
            AddValidator(uppercase,"uppercase", value => URegex.Uppercase(value));
            AddValidator(lowercase,"lowercase", value => URegex.Lowercase(value));
            AddValidator(number,"number", value => URegex.Number(value));
            AddValidator(specialCharacter,"special character", value => URegex.SpecialCharacter(value));

            void AddValidator(bool? state, string type, Func<string, bool> validator)
            {
                if(!state.HasValue) return;
                
                if(state.Value)
                    lst.Add(new UValidator(value => validator(value), $"Must be have at least one {type} character."));
                else
                    lst.Add(new UValidator(value => !validator(value), $"Does not contain an {type} character"));
            }

            return lst.ToArray();
        }

        public static UValidator[] Email(int lengthMin = 3) =>
            new UValidator[]
            {
                LengthMin(lengthMin),
                new UValidator(value => URegex.Email(value), "Must be a valid email address.")
            };
    }

    public record UValidators(UValidator[] validators,Func<string, (bool result, string errorMessage)>[] customValidators = null, Action<string> onError = null)
    {
        public bool Validate(string value) 
        {
            // Validators
            if(validators != null && validators.Length > 0)
                for (int i = 0; i < validators.Length; i++)
                    if (!validators[i].validate(value))
                    {
                        onError?.Invoke(validators[i].ErrorMessage);
                        return false;
                    }

            // Custom Validators
            if(customValidators != null && customValidators.Length > 0)
                for (int i = 0; i < customValidators.Length; i++)
                {
                    (bool result, string errorMessage) = customValidators[i](value);
                    if (!result)
                    {
                        onError?.Invoke(errorMessage);
                        return false;
                    }
                }
            
            onError?.Invoke(null);
            return true;
        }
    }
}