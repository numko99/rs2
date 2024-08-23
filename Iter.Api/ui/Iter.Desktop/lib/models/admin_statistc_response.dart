import 'package:json_annotation/json_annotation.dart';
import 'package:ui/models/reservation_diagram_response.dart';

part 'admin_statistc_response.g.dart';

@JsonSerializable()
class AdminStatisticResponse {
  int reservationCount;
  int arrangementCount;
  int usersCount;
  double totalAmount;
  int agenciesCount;
  List<ReservationDiagramResponse> reservations;

  AdminStatisticResponse({
    required this.reservationCount,
    required this.arrangementCount,
    required this.usersCount,
    required this.totalAmount,
    required this.agenciesCount,
    required this.reservations,
  });


  factory AdminStatisticResponse.fromJson(Map<String, dynamic> json) => _$AdminStatisticResponseFromJson(json);

  Map<String, dynamic> toJson() => _$AdminStatisticResponseToJson(this);
}
