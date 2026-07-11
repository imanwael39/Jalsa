using Bogus;

namespace Jalsa.Infrastructure.Data.Seeding;

/// <summary>
/// Definitions and scoring logic for the three standardized instruments the clinic uses:
/// PHQ-9 (depression), GAD-7 (anxiety) and BDI (Beck Depression Inventory).
///
/// PHQ-9 is already seeded by the <c>SeedPhq9AssessmentTemplate</c> migration, so its
/// definition here matches that migration verbatim; the seeder reuses the existing template
/// when present and only creates GAD-7 / BDI. Each item is scored 0..3 (Likert), and the
/// severity bands mirror <c>PatientAssessmentService.ComputeSeverity</c> for PHQ-9.
/// </summary>
internal sealed class AssessmentDefinition
{
    public required string Name { get; init; }
    public required int Version { get; init; }
    public required string Description { get; init; }
    public required string[] Questions { get; init; }
    public int MaxPerItem { get; init; } = 3;
    public required Func<int, string> Severity { get; init; }

    public int MaxTotal => Questions.Length * MaxPerItem;

    /// <summary>
    /// Distributes a target total score across the items, each within 0..MaxPerItem.
    /// Returns one answer value per question (same order as <see cref="Questions"/>).
    /// </summary>
    public int[] DistributeScore(int target, Faker faker)
    {
        target = Math.Clamp(target, 0, MaxTotal);
        var answers = new int[Questions.Length];
        var remaining = target;

        // Randomly sprinkle points onto items that still have headroom.
        while (remaining > 0)
        {
            var i = faker.Random.Int(0, answers.Length - 1);
            if (answers[i] >= MaxPerItem) continue;
            answers[i]++;
            remaining--;
        }
        return answers;
    }

    public static readonly AssessmentDefinition Phq9 = new()
    {
        Name = "phq-9",
        Version = 1,
        Description = "مقياس تقييم أعراض الاكتئاب المكون من 9 بنود (Patient Health Questionnaire-9)",
        Severity = score => score switch
        {
            <= 4 => "Minimal",
            <= 9 => "Mild",
            <= 14 => "Moderate",
            <= 19 => "ModeratelySevere",
            _ => "Severe",
        },
        Questions = new[]
        {
            "قلة الاهتمام أو المتعة في القيام بالأشياء",
            "الشعور بالحزن أو الاكتئاب أو اليأس",
            "صعوبة في النوم أو البقاء نائماً، أو النوم لفترة أطول من اللازم",
            "الشعور بالتعب أو قلة الطاقة",
            "ضعف الشهية أو الإفراط في الأكل",
            "الشعور بالسوء تجاه نفسك، أو أنك فاشل، أو أنك خذلت نفسك أو عائلتك",
            "صعوبة في التركيز على أشياء مثل قراءة الجريدة أو مشاهدة التلفاز",
            "التحرك أو التحدث ببطء شديد لدرجة أن الآخرين قد لاحظوا ذلك، أو العكس - الشعور بالتململ لدرجة التحرك أكثر من المعتاد",
            "أفكار بأنك ستكون أفضل حالاً لو كنت ميتاً، أو أفكار لإيذاء نفسك بطريقة ما",
        },
    };

    public static readonly AssessmentDefinition Gad7 = new()
    {
        Name = "gad-7",
        Version = 1,
        Description = "مقياس اضطراب القلق العام المكون من 7 بنود (Generalized Anxiety Disorder-7)",
        Severity = score => score switch
        {
            <= 4 => "Minimal",
            <= 9 => "Mild",
            <= 14 => "Moderate",
            _ => "Severe",
        },
        Questions = new[]
        {
            "الشعور بالعصبية أو القلق أو التوتر",
            "عدم القدرة على إيقاف القلق أو التحكم فيه",
            "القلق الزائد حول أمور مختلفة",
            "صعوبة في الاسترخاء",
            "التململ لدرجة صعوبة الجلوس بهدوء",
            "الانزعاج أو الغضب بسهولة",
            "الشعور بالخوف وكأن شيئاً سيئاً قد يحدث",
        },
    };

    public static readonly AssessmentDefinition Bdi = new()
    {
        Name = "bdi",
        Version = 1,
        Description = "قائمة بيك للاكتئاب المكونة من 21 بنداً (Beck Depression Inventory)",
        Severity = score => score switch
        {
            <= 13 => "Minimal",
            <= 19 => "Mild",
            <= 28 => "Moderate",
            _ => "Severe",
        },
        Questions = new[]
        {
            "الحزن", "التشاؤم بشأن المستقبل", "الشعور بالفشل", "فقدان المتعة",
            "الشعور بالذنب", "الشعور بالعقاب", "عدم الرضا عن النفس", "النقد الذاتي",
            "الأفكار المتعلقة بإيذاء النفس", "البكاء", "الهياج والتوتر", "فقدان الاهتمام بالآخرين",
            "التردد في اتخاذ القرارات", "الشعور بانعدام القيمة", "فقدان الطاقة", "تغيرات في نمط النوم",
            "سرعة الانفعال", "تغيرات في الشهية", "صعوبة التركيز", "التعب أو الإرهاق",
            "فقدان الاهتمام بالأنشطة اليومية",
        },
    };

    public static readonly AssessmentDefinition[] All = { Phq9, Gad7, Bdi };
}
