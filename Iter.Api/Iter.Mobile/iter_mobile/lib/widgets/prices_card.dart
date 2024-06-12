import 'package:flutter/material.dart';
import 'package:iter_mobile/models/arrangement_price.dart'; // Pretpostavka modela

class PriceDetail extends StatelessWidget {
  final ArrangementPrice price;

  const PriceDetail({Key? key, required this.price}) : super(key: key);

  @override
  Widget build(BuildContext context) {
      return Column(
        children: [
          SizedBox(height: 5),
          if (price.accommodationType != null)
            Row(
              mainAxisAlignment: MainAxisAlignment.start,
              children: [
                const Icon(Icons.payment, color: Colors.amber),
                SizedBox(width: 10),
                Text(
                  "${price.accommodationType!.toUpperCase()}: ",
                  style: TextStyle(
                      fontSize: 12.0,
                      fontWeight: FontWeight.bold,
                      color: Colors.grey[600]),
                ),
                Text("${price.price!.round()} KM",
                    style:
                        TextStyle(fontWeight: FontWeight.bold, fontSize: 13.0))
              ],
            ),
          if (price.accommodationType == null)
            Text("${price.price!.round()} KM",
                style: TextStyle(
                    fontSize: 20.0,
                    fontWeight: FontWeight.bold,
                    color: Colors.grey[700]))
        ],
      );
  }
}
