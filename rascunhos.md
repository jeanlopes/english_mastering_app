using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using OpenNLP.Tools.Parser;
using OpenNLP.Tools.Tokenize;
using OpenNLP.Uima;
using OpenNLP.Tools.PosTagger;
using OpenNLP.Tools.SentenceDetect;
using OpenNLP.Tools.Util;

namespace OpenSubtitlesCrawler
{
class Program
{
static void Main(string[] args)
{
Console.WriteLine("Digite o nome do filme ou seriado:");
string searchTerm = Console.ReadLine();

            Console.WriteLine("Digite o número da temporada (caso seja um seriado):");
            string seasonNumber = Console.ReadLine();

            // Obter os links das legendas
            List<string> subtitleLinks = GetSubtitleLinks(searchTerm, seasonNumber);

            // Baixar as legendas
            List<string> subtitles = DownloadSubtitles(subtitleLinks);

            // Processar as legendas
            Dictionary<string, string> wordDictionary = ProcessSubtitles(subtitles);

            // Exibir o dicionário
            foreach (var entry in wordDictionary)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
        }

        static List<string> GetSubtitleLinks(string searchTerm, string seasonNumber)
        {
            // Lógica para buscar e obter os links das legendas no OpenSubtitles
            // ...

            // Retornar uma lista de links de legenda
            return new List<string>();
        }

        static List<string> DownloadSubtitles(List<string> subtitleLinks)
        {
            List<string> subtitles = new List<string>();

            foreach (string link in subtitleLinks)
            {
                // Lógica para baixar as legendas a partir dos links
                // ...

                // Adicionar a legenda à lista
                subtitles.Add("Caminho/para/a/legenda.srt");
            }

            return subtitles;
        }

        static Dictionary<string, string> ProcessSubtitles(List<string> subtitles)
        {
            Dictionary<string, string> wordDictionary = new Dictionary<string, string>();

            // Configurar o modelo do OpenNLP
            string modelsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Models");
            var sentenceModel = new EnglishMaximumEntropySentenceDetector(modelsDirectory + "/EnglishSD.nbin");
            var tokenizerModel = new EnglishMaximumEntropyTokenizer(modelsDirectory + "/EnglishTok.nbin");
            var posTaggerModel = new EnglishMaximumEntropyPosTagger(modelsDirectory + "/EnglishPOS.nbin");
            var parserModel = new EnglishTreebankParser(modelsDirectory + "/englishPCFG.ser.gz");

            // Processar cada legenda
            foreach (string subtitlePath in subtitles)
            {
                // Ler o conteúdo do arquivo de legenda
                string subtitleContent = File.ReadAllText(subtitlePath);

                // Dividir o conteúdo em sentenças
                string[] sentences = sentenceModel.SentenceDetect(subtitleContent);

                // Processar cada sentença
                foreach (string sentence in sentences)
                {
                    // Tokenizar a sentença em palavras
                    string[] tokens = tokenizerModel.Tokenize(sentence);

                    // Executar o POS tagging nas palavras
                    string[] tags = posTaggerModel.Tag(tokens);

                    // Realizar o parsing da árvore de sintaxe
                    var parse = parserModel.DoParse(tokens, tags);

                    // Extrair informações relevantes das palavras da sentença
                    foreach (var word in parse.GetWords())
                    {
                        string wordText = word.Value;
                        string wordMeaning = ""; // Definir o significado da palavra usando uma biblioteca de dicionário

                        // Verificar se a palavra já está no dicionário
                        if (!wordDictionary.ContainsKey(wordText))
                        {
                            // Adicionar a palavra ao dicionário
                            wordDictionary.Add(wordText, wordMeaning);
                        }
                    }
                }
            }

            return wordDictionary;
        }
    }

}
