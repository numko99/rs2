import 'package:json_annotation/json_annotation.dart';

part 'reservation_diagram_response.g.dart';

@JsonSerializable()
class ReservationDiagramResponse {
  DateTime date;
  int reservationCount;

  ReservationDiagramResponse(
      {required this.date, required this.reservationCount});

  factory ReservationDiagramResponse.fromJson(Map<String, dynamic> json) =>
      _$ReservationDiagramResponseFromJson(json);

  Map<String, dynamic> toJson() => _$ReservationDiagramResponseToJson(this);
}
