using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XPlatformCloudKit.Helpers
{
    class HttpUtility
    {
        private static char[] _htmlEntityEndingChars = new char[] { ';', '&' };

        private static string[] _entitiesList = new string[]
        {
            "\"-quot",
            "&-amp",
            "'-apos",
            "<-lt",
            ">-gt",
            "\u00a0-nbsp",
            "¡-iexcl",
            "¢-cent",
            "£-pound",
            "¤-curren",
            "¥-yen",
            "¦-brvbar",
            "§-sect",
            "¨-uml",
            "©-copy",
            "ª-ordf",
            "«-laquo",
            "¬-not",
            "­-shy",
            "®-reg",
            "¯-macr",
            "°-deg",
            "±-plusmn",
            "²-sup2",
            "³-sup3",
            "´-acute",
            "µ-micro",
            "¶-para",
            "·-middot",
            "¸-cedil",
            "¹-sup1",
            "º-ordm",
            "»-raquo",
            "¼-frac14",
            "½-frac12",
            "¾-frac34",
            "¿-iquest",
            "À-Agrave",
            "Á-Aacute",
            "Â-Acirc",
            "Ã-Atilde",
            "Ä-Auml",
            "Å-Aring",
            "Æ-AElig",
            "Ç-Ccedil",
            "È-Egrave",
            "É-Eacute",
            "Ê-Ecirc",
            "Ë-Euml",
            "Ì-Igrave",
            "Í-Iacute",
            "Î-Icirc",
            "Ï-Iuml",
            "Ð-ETH",
            "Ñ-Ntilde",
            "Ò-Ograve",
            "Ó-Oacute",
            "Ô-Ocirc",
            "Õ-Otilde",
            "Ö-Ouml",
            "×-times",
            "Ø-Oslash",
            "Ù-Ugrave",
            "Ú-Uacute",
            "Û-Ucirc",
            "Ü-Uuml",
            "Ý-Yacute",
            "Þ-THORN",
            "ß-szlig",
            "à-agrave",
            "á-aacute",
            "â-acirc",
            "ã-atilde",
            "ä-auml",
            "å-aring",
            "æ-aelig",
            "ç-ccedil",
            "è-egrave",
            "é-eacute",
            "ê-ecirc",
            "ë-euml",
            "ì-igrave",
            "í-iacute",
            "î-icirc",
            "ï-iuml",
            "ð-eth",
            "ñ-ntilde",
            "ò-ograve",
            "ó-oacute",
            "ô-ocirc",
            "õ-otilde",
            "ö-ouml",
            "÷-divide",
            "ø-oslash",
            "ù-ugrave",
            "ú-uacute",
            "û-ucirc",
            "ü-uuml",
            "ý-yacute",
            "þ-thorn",
            "ÿ-yuml",
            "Œ-OElig",
            "œ-oelig",
            "Š-Scaron",
            "š-scaron",
            "Ÿ-Yuml",
            "ƒ-fnof",
            "ˆ-circ",
            "˜-tilde",
            "Α-Alpha",
            "Β-Beta",
            "Γ-Gamma",
            "Δ-Delta",
            "Ε-Epsilon",
            "Ζ-Zeta",
            "Η-Eta",
            "Θ-Theta",
            "Ι-Iota",
            "Κ-Kappa",
            "Λ-Lambda",
            "Μ-Mu",
            "Ν-Nu",
            "Ξ-Xi",
            "Ο-Omicron",
            "Π-Pi",
            "Ρ-Rho",
            "Σ-Sigma",
            "Τ-Tau",
            "Υ-Upsilon",
            "Φ-Phi",
            "Χ-Chi",
            "Ψ-Psi",
            "Ω-Omega",
            "α-alpha",
            "β-beta",
            "γ-gamma",
            "δ-delta",
            "ε-epsilon",
            "ζ-zeta",
            "η-eta",
            "θ-theta",
            "ι-iota",
            "κ-kappa",
            "λ-lambda",
            "μ-mu",
            "ν-nu",
            "ξ-xi",
            "ο-omicron",
            "π-pi",
            "ρ-rho",
            "ς-sigmaf",
            "σ-sigma",
            "τ-tau",
            "υ-upsilon",
            "φ-phi",
            "χ-chi",
            "ψ-psi",
            "ω-omega",
            "ϑ-thetasym",
            "ϒ-upsih",
            "ϖ-piv",
            "\u2002-ensp",
            "\u2003-emsp",
            "\u2009-thinsp",
            "‌-zwnj",
            "‍-zwj",
            "‎-lrm",
            "‏-rlm",
            "–-ndash",
            "—-mdash",
            "‘-lsquo",
            "’-rsquo",
            "‚-sbquo",
            "“-ldquo",
            "”-rdquo",
            "„-bdquo",
            "†-dagger",
            "‡-Dagger",
            "•-bull",
            "…-hellip",
            "‰-permil",
            "′-prime",
            "″-Prime",
            "‹-lsaquo",
            "›-rsaquo",
            "‾-oline",
            "⁄-frasl",
            "€-euro",
            "ℑ-image",
            "℘-weierp",
            "ℜ-real",
            "™-trade",
            "ℵ-alefsym",
            "←-larr",
            "↑-uarr",
            "→-rarr",
            "↓-darr",
            "↔-harr",
            "↵-crarr",
            "⇐-lArr",
            "⇑-uArr",
            "⇒-rArr",
            "⇓-dArr",
            "⇔-hArr",
            "∀-forall",
            "∂-part",
            "∃-exist",
            "∅-empty",
            "∇-nabla",
            "∈-isin",
            "∉-notin",
            "∋-ni",
            "∏-prod",
            "∑-sum",
            "−-minus",
            "∗-lowast",
            "√-radic",
            "∝-prop",
            "∞-infin",
            "∠-ang",
            "∧-and",
            "∨-or",
            "∩-cap",
            "∪-cup",
            "∫-int",
            "∴-there4",
            "∼-sim",
            "≅-cong",
            "≈-asymp",
            "≠-ne",
            "≡-equiv",
            "≤-le",
            "≥-ge",
            "⊂-sub",
            "⊃-sup",
            "⊄-nsub",
            "⊆-sube",
            "⊇-supe",
            "⊕-oplus",
            "⊗-otimes",
            "⊥-perp",
            "⋅-sdot",
            "⌈-lceil",
            "⌉-rceil",
            "⌊-lfloor",
            "⌋-rfloor",
            "〈-lang",
            "〉-rang",
            "◊-loz",
            "♠-spades",
            "♣-clubs",
            "♥-hearts",
            "♦-diams"
        };

        private static Dictionary<string, char> _lookupTable = GenerateLookupTable();

        private static Dictionary<string, char> GenerateLookupTable()
        {
            Dictionary<string, char> dictionary = new Dictionary<string, char>(StringComparer.Ordinal);
            string[] entitiesList = _entitiesList;
            for (int i = 0; i < entitiesList.Length; i++)
            {
                string text = entitiesList[i];
                dictionary.Add(text.Substring(2), text[0]);
            }
            return dictionary;
        }

        public static char Lookup(string entity)
        {
            char result;
            _lookupTable.TryGetValue(entity, out result);
            return result;
        }

        public static string HtmlDecode(string html)
        {
            if (html == null)
            {
                return null;
            }
            if (html.IndexOf('&') < 0)
            {
                return html;
            }
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb, CultureInfo.InvariantCulture);
            int length = html.Length;
            for (int i = 0; i < length; i++)
            {
                char ch = html[i];
                if (ch == '&')
                {
                    int num3 = html.IndexOfAny(_htmlEntityEndingChars, i + 1);
                    if ((num3 > 0) && (html[num3] == ';'))
                    {
                        string entity = html.Substring(i + 1, (num3 - i) - 1);
                        if ((entity.Length > 1) && (entity[0] == '#'))
                        {
                            try
                            {
                                if ((entity[1] == 'x') || (entity[1] == 'X'))
                                {
                                    ch = (char)int.Parse(entity.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    ch = (char)int.Parse(entity.Substring(1), CultureInfo.InvariantCulture);
                                }
                                i = num3;
                            }
                            catch (FormatException)
                            {
                                i++;
                            }
                            catch (ArgumentException)
                            {
                                i++;
                            }
                        }
                        else
                        {
                            i = num3;
                            char ch2 = Lookup(entity);
                            if (ch2 != '\0')
                            {
                                ch = ch2;
                            }
                            else
                            {
                                writer.Write('&');
                                writer.Write(entity);
                                writer.Write(';');
                                continue;
                            }
                        }
                    }
                }
                writer.Write(ch);
            }
            return sb.ToString();
        }
    }
}
