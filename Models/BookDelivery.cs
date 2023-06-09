using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SrmBook.Models
{
    public class BookDelivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DELIVERY_NUM { get; set; }

        [Display(Name = "발주 번호")]
        public int ORDER_NUM { get; set; }

        [Display(Name = "배송일자")]
        public DateTime DELIVERY_DATE { get; set; }

        [Display(Name = "출판사")]
        public string PUBLISHER { get; set; }

        [Display(Name = "배송 주소")]
        public string ADDRESS { get; set; }

        [Display(Name = "연락처")]
        public string CONTACT { get; set; }

        // 배송 상태
        [Display(Name = "배송 상태")]
        public DeliveryStatus STATUS { get; set; }

        // 관계 설정: 배송은 하나의 주문에 속하며, 주문과 일대일 관계를 가짐
        [ForeignKey("ORDER_NUM")]
        public virtual BookOrder Order { get; set; }
    }

    // 배송 상태 열거형
    public enum DeliveryStatus
    {
        [Display(Name = "배송 준비 중")]
        Pending,

        [Display(Name = "배송 중")]
        InTransit,

        [Display(Name = "배송 완료")]
        Delivered
    }
}
