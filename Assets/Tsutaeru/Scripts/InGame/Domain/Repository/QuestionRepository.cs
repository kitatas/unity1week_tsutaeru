using System;
using Tsutaeru.InGame.Data.DataStore;
using UniEx;

namespace Tsutaeru.InGame.Domain.Repository
{
    public sealed class QuestionRepository
    {
        private readonly QuestionTable _questionTable;

        public QuestionRepository(QuestionTable questionTable)
        {
            _questionTable = questionTable;
        }

        /// <summary>
        /// 指定した難易度から対象の問題を1つ取得する
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public QuestionData Find(Difficulty difficulty)
        {
            var target = _questionTable.data.FindAll(x => x.difficulty == difficulty);
            if (target.Count == 0)
            {
                throw new Exception($"target question is nothing. (difficulty: {difficulty})");
            }

            var data = target.GetRandom();
            if (data == null)
            {
                throw new Exception($"question data is null.");
            }

            if (string.IsNullOrEmpty(data.origin) || string.IsNullOrEmpty(data.originHint))
            {
                throw new Exception($"question origin data is invalid. (name: {data.name})");
            }

            if (string.IsNullOrEmpty(data.answer) || string.IsNullOrEmpty(data.answerHint))
            {
                throw new Exception($"question answer data is invalid. (name: {data.name})");
            }

            if (data.origin.Length != data.answer.Length)
            {
                throw new Exception($"question data is invalid. (name: {data.name})");
            }

            return data;
        }
    }
}